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
    public class AirlineCompanyFacadeController : ControllerBase
    {
        [HttpGet(nameof(GetAllTickets))]
        public IActionResult GetAllTickets()
        {
            throw new NotImplementedException();
        }

        [HttpGet(nameof(GetAllFlights))]
        public IActionResult GetAllFlights()
        {
            throw new NotImplementedException();
        }

        [HttpDelete(nameof(CancelFlight))]
        public IActionResult CancelFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        [HttpPost(nameof(CreateFlight))]
        public IActionResult CreateFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        [HttpPut(nameof(UpdateFlight))]
        public IActionResult UpdateFlight(Flight flight)
        {
            throw new NotImplementedException();
        }

        [HttpPut(nameof(ChangeMyPassword))]
        public IActionResult ChangeMyPassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        [HttpPut(nameof(MofidyAirlineDetails))]
        public IActionResult MofidyAirlineDetails(AirlineCompany airline)
        {
            throw new NotImplementedException();
        }
    }
}