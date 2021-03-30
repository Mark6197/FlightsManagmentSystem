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
        [HttpGet]
        IActionResult GetAllFlights()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetAllAirlineCompanies()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetAllFlightsVacancy()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetFlightById(int id)
        {
            throw new NotImplementedException();

        }

        [HttpGet]
        IActionResult GetFlightsByOriginCountry(int countryCode)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetFlightsByDestinationCountry(int countryCode)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetFlightsByDepatrureDate(DateTime departureDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        IActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            throw new NotImplementedException();
        }
    }
}
