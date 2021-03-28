using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
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

        }

        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({ticket})");

            try
            {
                if (token.User != ticket.Customer)
                    throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to cancel ticket {ticket.Id} that belogns to {ticket.Customer.User.UserName}");

                _ticketDAO.Remove(ticket);
                Flight flight = _flightDAO.Get((int)ticket.Id);

                lock (key)
                {
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

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({token}, {flight})");
            Ticket ticket = null;
            Flight flight_from_db = _flightDAO.Get((int)flight.Id);

            if (flight_from_db.RemainingTickets <= 0)
                throw new TicketPurchaseFailedException($"User {token.User.User.UserName} failed to purchase ticket to flight {flight.Id}. No tickets left");

            lock (key)
            {
                flight_from_db = _flightDAO.Get((int)flight.Id);

                if (flight_from_db.RemainingTickets <= 0)
                    throw new TicketPurchaseFailedException($"User {token.User.User.UserName} failed to purchase ticket to flight {flight.Id}. No tickets left");

                ticket = new Ticket(flight, token.User);
                long ticket_id = _ticketDAO.Add(ticket);
                ticket.Id = ticket_id;

                flight_from_db.RemainingTickets--;
                _flightDAO.Update(flight_from_db);
            }

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {ticket}");

            return ticket;
        }
    }
}
