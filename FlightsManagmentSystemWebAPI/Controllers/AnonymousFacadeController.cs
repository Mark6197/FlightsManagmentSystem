using BL;
using BL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnonymousFacadeController : ControllerBase
    {
        private readonly IFlightCenterSystem _flightCenterSystem= FlightCenterSystem.GetInstance();
        private readonly IAnonymousUserFacade _anonymousUserFacade;
        private readonly ILogger<AnonymousFacadeController> _logger;

        public AnonymousFacadeController(ILogger<AnonymousFacadeController> logger)
        {
            _anonymousUserFacade = _flightCenterSystem.GetFacade<AnonymousUserFacade>();
            _logger = logger;
        }

        [HttpGet("GetAllFlights")]
        public ActionResult<IList<Flight>> GetAllFlights()
        {
            IList<Flight> flights=_anonymousUserFacade.GetAllFlights();
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);        
        }

        [HttpGet("GetAllAirlineCompanies")]
        public ActionResult<IList<AirlineCompany>> GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlineCompanies = _anonymousUserFacade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0)
                return NoContent();

            return Ok(airlineCompanies);
        }

        [HttpGet("GetAllFlightsVacancy")]
        public ActionResult<IDictionary<Flight, int>> GetAllFlightsVacancy()
        {
            IDictionary<Flight,int> flights_vacancy = _anonymousUserFacade.GetAllFlightsVacancy();
            if (flights_vacancy.Count == 0)
                return NoContent();

            return Ok(flights_vacancy);
        }

        [HttpGet("GetAllCountries")]
        public ActionResult<IList<Country>> GetAllCountries()
        {
            IList<Country> countries = _anonymousUserFacade.GetAllCountries();
            if (countries.Count == 0)
                return NoContent();

            return Ok(countries);
        }

        [HttpGet("GetAirlineCompanyById")]
        public ActionResult<AirlineCompany> GetAirlineCompanyById(long id)
        {
            AirlineCompany airlineCompany = _anonymousUserFacade.GetAirlineCompanyById(id);
            if (airlineCompany == null)
                return NotFound();

            return Ok(airlineCompany);
        }

        [HttpGet("GetFlightById")]
        public ActionResult<Flight> GetFlightById(long id)
        {
            Flight flight = _anonymousUserFacade.GetFlightById(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpGet("GetCountryById")]
        public ActionResult<Country> GetCountryById(int id)
        {
            Country country = _anonymousUserFacade.GetCountryById(id);
            if (country == null)
                return NotFound();

            return Ok(country);
        }

        [HttpGet("GetFlightsByOriginCountry")]
        public ActionResult<IList<Flight>> GetFlightsByOriginCountry(int countryId)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByOriginCountry(countryId);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByDestinationCountry")]
        public ActionResult<IList<Flight>> GetFlightsByDestinationCountry(int countryId)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByDestinationCountry(countryId);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByDepatrureDate")]
        public ActionResult<IList<Flight>> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByDepatrureDate(departureDate);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByLandingDate")]
        public ActionResult<IList<Flight>> GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByLandingDate(landingDate);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }
    }
}
