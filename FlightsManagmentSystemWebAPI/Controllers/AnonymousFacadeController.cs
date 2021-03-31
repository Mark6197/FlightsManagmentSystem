using BL;
using BL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public AnonymousFacadeController()
        {
            //_flightCenterSystem = flightCenterSystem;
            _anonymousUserFacade = _flightCenterSystem.GetFacade<AnonymousUserFacade>();
        }

        [HttpGet("GetAllFlights")]
        public IActionResult GetAllFlights()
        {
            IList<Flight> flights=_anonymousUserFacade.GetAllFlights();
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);        
        }

        [HttpGet("GetAllAirlineCompanies")]
        public IActionResult GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlineCompanies = _anonymousUserFacade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0)
                return NoContent();

            return Ok(airlineCompanies);
        }

        [HttpGet("GetAllFlightsVacancy")]
        public IActionResult GetAllFlightsVacancy()
        {
            IDictionary<Flight,int> flights_vacancy = _anonymousUserFacade.GetAllFlightsVacancy();
            if (flights_vacancy.Count == 0)
                return NoContent();

            return Ok(flights_vacancy);
        }

        [HttpGet("GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            IList<Country> countries = _anonymousUserFacade.GetAllCountries();
            if (countries.Count == 0)
                return NoContent();

            return Ok(countries);
        }

        [HttpGet("GetAirlineCompanyById")]
        public IActionResult GetAirlineCompanyById(int id)//maybe change to long later
        {
            AirlineCompany airlineCompany = _anonymousUserFacade.GetAirlineCompanyById(id);
            if (airlineCompany == null)
                return NotFound();

            return Ok(airlineCompany);
        }

        [HttpGet("GetFlightById")]
        public IActionResult GetFlightById(int id)//maybe change to long later
        {
            Flight flight = _anonymousUserFacade.GetFlightById(id);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpGet("GetCountryById")]
        public IActionResult GetCountryById(int id)
        {
            Country country = _anonymousUserFacade.GetCountryById(id);
            if (country == null)
                return NotFound();

            return Ok(country);
        }

        [HttpGet("GetFlightsByOriginCountry")]
        public IActionResult GetFlightsByOriginCountry(int countryId)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByOriginCountry(countryId);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByDestinationCountry")]
        public IActionResult GetFlightsByDestinationCountry(int countryId)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByDestinationCountry(countryId);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByDepatrureDate")]
        public IActionResult GetFlightsByDepatrureDate(DateTime departureDate)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByDepatrureDate(departureDate);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }

        [HttpGet("GetFlightsByLandingDate")]
        public IActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = _anonymousUserFacade.GetFlightsByLandingDate(landingDate);
            if (flights.Count == 0)
                return NoContent();

            return Ok(flights);
        }
    }
}
