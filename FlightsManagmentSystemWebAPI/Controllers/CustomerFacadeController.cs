using BL;
using BL.Interfaces;
using BL.LoginService;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFacadeController : ControllerBase
    {
        private readonly IFlightCenterSystem _flightCenterSystem = FlightCenterSystem.GetInstance();// I am not using the interface because i want access to the private login service. might want to reconsider 
        private readonly ILoggedInCustomerFacade _loggedInCustomerFacade;
        private readonly LinkGenerator _linkGenerator;

        public CustomerFacadeController(LinkGenerator linkGenerator)
        {
            _loggedInCustomerFacade = _flightCenterSystem.GetFacade<LoggedInCustomerFacade>();
            _linkGenerator = linkGenerator;
        }

        private LoginToken<Customer> getLoginToken()
        {
            string user_name = "customerA";
            string password = "AAAAAAA";
            bool is_success = _flightCenterSystem.TryLogin(user_name, password, out ILoginToken token, out FacadeBase facade);

            if (is_success)
            {
                LoginToken<Customer> customer_token = token as LoginToken<Customer>;
                return customer_token;
            }

            return null;
        }

        [HttpGet("GetAllMyFlights")]
        public ActionResult<Flight> GetAllMyFlights()
        {
            LoginToken<Customer> customer_token = getLoginToken();

            IList<Flight> flights = _loggedInCustomerFacade.GetAllMyFlights(customer_token);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet(nameof(GetAllMyTickets))]
        public IActionResult GetAllMyTickets()
        {
            LoginToken<Customer> customer_token = getLoginToken();

            IList<Ticket> tickets = _loggedInCustomerFacade.GetAllMyTickets(customer_token);
            if (tickets.Count == 0)
                return NoContent();

            return Ok(tickets);
        }

        [HttpGet(nameof(GetTicketById))]
        public IActionResult GetTicketById(long id)
        {
            LoginToken<Customer> customer_token = getLoginToken();
            Ticket ticket = null;
            try
            {
                ticket = _loggedInCustomerFacade.GetTicketById(customer_token, id);
                if (ticket == null)
                    return NotFound();

            }
            catch (Exception)
            {
                throw;
            }

            return Ok(ticket);
        }

        [HttpPost("PurchaseTicket")]
        public IActionResult PurchaseTicket(Flight flight)
        {
            LoginToken<Customer> customer_token = getLoginToken();
            Ticket ticket = null;
            string uri = null;
            try
            {
                ticket = _loggedInCustomerFacade.PurchaseTicket(customer_token, flight);
                if (ticket == null)
                    return StatusCode(StatusCodes.Status410Gone);

                uri = _linkGenerator.GetPathByAction(nameof(GetTicketById),"customerfacade", new { id = ticket.Id });

            }
            catch (Exception)
            {
                throw;
            }

            return Created(uri, ticket);
        }

        [HttpDelete("CancelTicket")]
        public IActionResult CancelTicket(Ticket ticket)
        {
            LoginToken<Customer> customer_token = getLoginToken();
            try
            {
                _loggedInCustomerFacade.CancelTicket(customer_token, ticket);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }
    }
}
