using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using Domain.Entities;
using System.Collections.Generic;

namespace BL
{
    class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        public LoggedInCustomerFacade():base()
        {

        }

        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            try
            {
                if (token.User != ticket.Customer)
                    throw new NotAllowedCustomerActionException($"Customer {token.User.User.UserName} not allowed to cancel ticket {ticket.Id} that belogns to {ticket.Customer.User.UserName}");

                _ticketDAO.Remove(ticket);
            }
            catch (NotAllowedCustomerActionException)
            {

                throw;
            }
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            return _flightDAO.GetFlightsByCustomer(token.User);
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            Ticket ticket = new Ticket(flight, token.User);
            _ticketDAO.Add(ticket);
            return ticket;//problem with id
        }
    }
}
