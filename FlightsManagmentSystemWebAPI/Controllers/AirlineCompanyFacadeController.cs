using BL;
using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineCompanyFacadeController : LoggedInControllerBase<AirlineCompany>
    {
        private readonly IFlightCenterSystem _flightCenterSystem = FlightCenterSystem.GetInstance();
        private readonly ILoggedInAirlineFacade _loggedInAirlineFacade;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<AirlineCompanyFacadeController> _logger;

        public AirlineCompanyFacadeController(LinkGenerator linkGenerator, ILogger<AirlineCompanyFacadeController> logger)
        {
            _loggedInAirlineFacade = _flightCenterSystem.GetFacade<LoggedInAirlineFacade>();
            _linkGenerator = linkGenerator;
            _logger = logger;
        }

        [HttpGet(nameof(GetAllTickets))]
        public ActionResult<IList<Ticket>> GetAllTickets()
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();

            IList<Ticket> tickets = _loggedInAirlineFacade.GetAllTickets(airline_token);
            if (tickets.Count == 0)
                return NoContent();

            return Ok(tickets);
        }

        [HttpGet(nameof(GetAllTicketsByFlight))]
        public ActionResult<IList<Ticket>> GetAllTicketsByFlight(Flight flight)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();
            IList<Ticket> tickets = null;
            try
            {
                tickets = _loggedInAirlineFacade.GetAllTicketsByFlight(airline_token, flight);
                if (tickets.Count == 0)
                    return NoContent();
            }
            catch (NotAllowedAirlineActionException)
            {
                return Unauthorized();
            }

            return Ok(tickets);
        }

        [HttpGet(nameof(GetAllFlights))]
        public ActionResult<IList<Flight>> GetAllFlights()
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();

            IList<Flight> flights = _loggedInAirlineFacade.GetAllFlights(airline_token);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpDelete(nameof(CancelFlight))]
        public IActionResult CancelFlight(Flight flight)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();
            try
            {
                _loggedInAirlineFacade.CancelFlight(airline_token, flight);
            }
            catch (NotAllowedAirlineActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPost(nameof(CreateFlight))]
        public ActionResult<Flight> CreateFlight(Flight flight)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();
            string uri = null;
            try
            {
                flight.Id = _loggedInAirlineFacade.CreateFlight(airline_token, flight);
                if (flight.Id == 0)
                    return Conflict();

                uri = _linkGenerator.GetPathByAction(nameof(AnonymousFacadeController.GetFlightById), "AnonymousFacade", new { id = flight.Id });

            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return Created(uri, flight);
        }

        [HttpPut(nameof(UpdateFlight))]
        public IActionResult UpdateFlight(Flight flight)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();

            try
            {
                if (flight.Id == 0)
                    return NotFound();

                _loggedInAirlineFacade.UpdateFlight(airline_token, flight);
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }
            catch (NotAllowedAirlineActionException)
            {
                return Unauthorized();
            }
            return NoContent();
        }

        [HttpPut(nameof(ChangeMyPassword))]
        public IActionResult ChangeMyPassword(string oldPassword, string newPassword)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();

            if (oldPassword == newPassword)
                return BadRequest();

            try
            {
                _loggedInAirlineFacade.ChangeMyPassword(airline_token, oldPassword, newPassword);
            }
            catch (WrongPasswordException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut(nameof(MofidyAirlineDetails))]
        public IActionResult MofidyAirlineDetails(AirlineCompany airline)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();

            try
            {
                _loggedInAirlineFacade.MofidyAirlineDetails(airline_token, airline);
            }
            catch (NotAllowedAirlineActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpGet(nameof(GetFlightHistoryByOriginalId))]
        public ActionResult<FlightHistory> GetFlightHistoryByOriginalId(long original_id)
        {
            LoginToken<AirlineCompany> airline_token = DesirializeToken();
            FlightHistory flightHistory = null;

            try
            {
                flightHistory = _loggedInAirlineFacade.GetFlightHistoryByOriginalId(airline_token, original_id);
                if (flightHistory == null)
                    return NotFound();
            }
            catch (NotAllowedAirlineActionException)
            {
                return Unauthorized();
            }

            return Ok(flightHistory);
        }

    }
}