using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL;
using Domain.Entities;
using System.Collections.Generic;
using System.Reflection;

namespace BL
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private object key = new object();

        public LoggedInCustomerFacade() : base()
        {
            _ticketDAO = new TicketDAOPGSQL();
        }

        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({ticket})");

            try
            {
                if (token.User != ticket.Customer)
                    throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to cancel ticket {ticket.Id} that belogns to customer with id {ticket.Customer.Id}");

                _ticketDAO.Remove(ticket);

                lock (key)//Lock this critical section so that there won't be adding and subtract at the same time 
                {
                    Flight flight = _flightDAO.Get((int)ticket.Flight.Id);
                    flight.RemainingTickets++;
                    _flightDAO.Update(flight);
                }
            }
            //catch (NotAllowedCustomerActionException)
            //{

            //    throw;
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _flightDAO.GetFlightsByCustomer(token.User);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Ticket> GetAllMyTickets(LoginToken<Customer> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _ticketDAO.Run_Generic_SP("sp_get_all_tickets_by_customer", new { _customer_id = token.User.Id }, true);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public Ticket GetTicketById(LoginToken<Customer> token, long id)
        {
            Ticket ticket = null;

            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _ticketDAO.Run_Generic_SP("sp_get_ticket", new { _id = id }, true);

            if (result.Count > 0)
            {
                if (token.User != result[0].Customer)
                    throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to get details of ticket {result[0].Id} that belogns to customer with id {result[0].Customer.Id}");

                ticket = result[0];
            }

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {ticket}");

            return ticket;
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({token}, {flight})");
            Ticket ticket = null;
            Flight flight_from_db = _flightDAO.Get((int)flight.Id);

            if (flight_from_db.RemainingTickets <= 0)//If there are no tickets left throw exception
                throw new TicketPurchaseFailedException($"User {token.User.User.UserName} failed to purchase ticket to flight {flight.Id}. No tickets left");

            lock (key)//If there are tickets lock this critical section 
            {
                flight_from_db = _flightDAO.Get((int)flight.Id);//Get the flight from the db again

                if (flight_from_db.RemainingTickets <= 0)//Make sure that there are still tickets left
                    throw new TicketPurchaseFailedException($"User {token.User.User.UserName} failed to purchase ticket to flight {flight.Id}. No tickets left");

                flight_from_db.RemainingTickets--;//Remove one ticket from the remaining tickets
                _flightDAO.Update(flight_from_db);//Update the flight
            }

            ticket = new Ticket(flight_from_db, token.User);//Create new ticket
            long ticket_id = _ticketDAO.Add(ticket);//Add the ticket
            ticket.Id = ticket_id;


            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {ticket}");

            return ticket;
        }
    }
}
