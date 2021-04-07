using BL;
using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerFacadeController : LoggedInControllerBase<Customer>
    {
        private readonly IFlightCenterSystem _flightCenterSystem = FlightCenterSystem.GetInstance(); 
        private readonly ILoggedInCustomerFacade _loggedInCustomerFacade;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<CustomerFacadeController> _logger;

        public CustomerFacadeController(LinkGenerator linkGenerator, ILogger<CustomerFacadeController> logger)
        {
            _loggedInCustomerFacade = _flightCenterSystem.GetFacade<LoggedInCustomerFacade>();
            _linkGenerator = linkGenerator;
            _logger = logger;
        }


        [HttpGet(nameof(GetAllMyFlights))]
        public ActionResult<Flight> GetAllMyFlights()
        {
                _logger.LogInformation("TEST");
                //_logger.LogDebug($"Enter {MethodBase.GetCurrentMethod().Name}");
                LoginToken<Customer> customer_token = DesirializeToken();

                IList<Flight> flights = _loggedInCustomerFacade.GetAllMyFlights(customer_token);
                if (flights.Count == 0)
                    return NoContent();

                return Ok(flights);
            
           
        }

        [HttpGet(nameof(GetAllMyTickets))]
        public IActionResult GetAllMyTickets()
        {
            LoginToken<Customer> customer_token = DesirializeToken();

            IList<Ticket> tickets = _loggedInCustomerFacade.GetAllMyTickets(customer_token);
            if (tickets.Count == 0)
                return NoContent();

            return Ok(tickets);
        }

        [HttpGet(nameof(GetTicketById))]
        public IActionResult GetTicketById(long id)
        {
            LoginToken<Customer> customer_token = DesirializeToken();
            Ticket ticket = null;
            try
            {
                ticket = _loggedInCustomerFacade.GetTicketById(customer_token, id);
                if (ticket == null)
                    return NotFound();

            }
            catch (WrongCustomerException)
            {
                return Unauthorized();
            }

            return Ok(ticket);
        }

        [HttpPost(nameof(PurchaseTicket))]
        public ActionResult<Ticket> PurchaseTicket(Flight flight)
        {
            LoginToken<Customer> customer_token = DesirializeToken();
            Ticket ticket = null;
            string uri = null;
            try
            {
                ticket = _loggedInCustomerFacade.PurchaseTicket(customer_token, flight);
                if (ticket == null)
                    return StatusCode(StatusCodes.Status410Gone);

                uri = _linkGenerator.GetPathByAction(nameof(GetTicketById), "customerfacade", new { id = ticket.Id });

            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return Created(uri, ticket);
        }

        [HttpDelete(nameof(CancelTicket))]
        public IActionResult CancelTicket(Ticket ticket)
        {
            LoginToken<Customer> customer_token = DesirializeToken();
            try
            {
                _loggedInCustomerFacade.CancelTicket(customer_token, ticket);
            }
            catch (WrongCustomerException)
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
