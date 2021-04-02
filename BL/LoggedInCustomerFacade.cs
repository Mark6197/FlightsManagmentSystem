﻿using BL.Exceptions;
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
        }

        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            Execute(() =>
            {
                if (token.User != ticket.Customer)
                    throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to cancel ticket {ticket.Id} that belogns to customer with id {ticket.Customer.Id}");

                _ticketDAO.Remove(ticket);

                Flight flight = _flightDAO.Get(ticket.Flight.Id);
                flight.RemainingTickets++;//maybe add this to the procedure of the cancel
                _flightDAO.Update(flight);
            }, new { Token = token, Ticket = ticket }, _logger);
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByCustomer(token.User), new { Token = token }, _logger);

            return result;
        }

        public IList<Ticket> GetAllMyTickets(LoginToken<Customer> token)
        {
            IList<Ticket> result = null;

            result = Execute(() => 
                        _ticketDAO.Run_Generic_SP("sp_get_all_tickets_by_customer", new { _customer_id = token.User.Id }, true),
            new { Token = token }, _logger);

            return result;
        }

        public Ticket GetTicketById(LoginToken<Customer> token, long id)
        {
            Ticket result = null;

            result = Execute(() =>
            {
                var ticket = _ticketDAO.Run_Generic_SP("sp_get_ticket", new { _id = id }, true);

                if (ticket.Count > 0)
                {
                    if (token.User != ticket[0].Customer)
                        throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to get details of ticket {ticket[0].Id} that belogns to customer with id {ticket[0].Customer.Id}");

                    return ticket[0];
                }
                return null;
            }, new { Token = token, Id=id }, _logger);

            return result;
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            Ticket result = null;

            result = Execute(() =>
            {
                Flight flight_from_db = _flightDAO.Get(flight.Id);

                if (flight_from_db.RemainingTickets <= 0)//If there are no tickets left throw exception
                    throw new TicketPurchaseFailedException($"User {token.User.User.UserName} failed to purchase ticket to flight {flight.Id}. No tickets left");

                //maybe add this to the procedure of add ticket
                flight_from_db.RemainingTickets--;//Remove one ticket from the remaining tickets
                _flightDAO.Update(flight_from_db);//Update the flight


                Ticket ticket = new Ticket(flight_from_db, token.User);//Create new ticket
                long ticket_id = _ticketDAO.Add(ticket);//Add the ticket
                ticket.Id = ticket_id;
                
                return ticket;
            }, new { Token = token, Flight = flight }, _logger);

            return result;
        }
    }
}
