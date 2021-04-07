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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsManagmentSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorFacadeController : LoggedInControllerBase<Administrator>
    {
        private readonly IFlightCenterSystem _flightCenterSystem = FlightCenterSystem.GetInstance();
        private readonly ILoggedInAdministratorFacade _loggedInAdministratorFacade;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<AdministratorFacadeController> _logger;

        public AdministratorFacadeController(LinkGenerator linkGenerator, ILogger<AdministratorFacadeController> logger)
        {
            _loggedInAdministratorFacade = _flightCenterSystem.GetFacade<LoggedInAdministratorFacade>();
            _linkGenerator = linkGenerator;
            _logger = logger;
        }

        [HttpGet(nameof(GetAllCustomers))]
        public ActionResult<IList<Customer>> GetAllCustomers()
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            IList<Customer> customers = _loggedInAdministratorFacade.GetAllCustomers(admin_token);
            if (customers.Count == 0)
                return NoContent();

            return Ok(customers);
        }

        [HttpPost(nameof(CreateNewAirlineCompany))]
        public ActionResult<AirlineCompany> CreateNewAirlineCompany(AirlineCompany airline)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();
            string uri = null;
            try
            {
                airline.Id = _loggedInAdministratorFacade.CreateNewAirlineCompany(admin_token, airline);
                if (airline.Id == 0)
                    return Conflict();

                uri = _linkGenerator.GetPathByAction(nameof(AnonymousFacadeController.GetAirlineCompanyById), "AnonymousFacade", new { id = airline.Id });

            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return Created(uri, airline);
        }

        [HttpPut(nameof(UpdateAirlineDetails))]
        public IActionResult UpdateAirlineDetails(AirlineCompany airline)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                if (airline.Id == 0)
                    return NotFound();

                _loggedInAdministratorFacade.UpdateAirlineDetails(admin_token, airline);
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }
        
            return NoContent();
        }

        [HttpDelete(nameof(RemoveAirline))]
        public IActionResult RemoveAirline(AirlineCompany airline)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                _loggedInAdministratorFacade.RemoveAirline(admin_token, airline);
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPost(nameof(CreateNewCustomer))]
        public ActionResult<Customer> CreateNewCustomer(Customer customer)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            string uri = null;
            try
            {
                customer.Id = _loggedInAdministratorFacade.CreateNewCustomer(admin_token, customer);
                if (customer.Id == 0)
                    return Conflict();

                uri = _linkGenerator.GetPathByAction(nameof(AnonymousFacadeController.GetAirlineCompanyById), "AnonymousFacade", new { id = customer.Id });
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return Created(uri, customer);
        }

        [HttpPut(nameof(UpdateCustomerDetails))]
        public IActionResult UpdateCustomerDetails(Customer customer)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                if (customer.Id == 0)
                    return NotFound();

                _loggedInAdministratorFacade.UpdateCustomerDetails(admin_token, customer);
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }
            return NoContent();
        }

        [HttpDelete(nameof(RemoveCustomer))]
        public IActionResult RemoveCustomer(Customer customer)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                _loggedInAdministratorFacade.RemoveCustomer(admin_token, customer);
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPost(nameof(CreateNewAdmin))]
        public ActionResult<Administrator> CreateNewAdmin(Administrator admin)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();
            string uri = null;
            try
            {
                admin.Id = _loggedInAdministratorFacade.CreateNewAdmin(admin_token, admin);
                if (admin.Id == 0)
                    return Conflict();

                uri = _linkGenerator.GetPathByAction(nameof(AnonymousFacadeController.GetAirlineCompanyById), "AnonymousFacade", new { id = admin.Id });
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return Created(uri, admin);
        }

        [HttpPost(nameof(CreateNewCountry))]
        public ActionResult<Country> CreateNewCountry(Country country)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();
            string uri = null;
            try
            {
                country.Id = _loggedInAdministratorFacade.CreateNewCountry(admin_token, country);
                if (country.Id == 0)
                    return Conflict();

                uri = _linkGenerator.GetPathByAction(nameof(AnonymousFacadeController.GetAirlineCompanyById), "AnonymousFacade", new { id = country.Id });
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return Created(uri, country);

        }

        [HttpGet(nameof(GetAdminById))]
        public ActionResult<Administrator> GetAdminById(int id)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            Administrator admin = _loggedInAdministratorFacade.GetAdminById(admin_token, id);
            if (admin == null)
                return NotFound();

            return Ok(admin);
        }

        [HttpGet(nameof(GetCustomerById))]
        public ActionResult<Customer> GetCustomerById(long id)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            Customer customer = _loggedInAdministratorFacade.GetCustomerById(admin_token, id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPut(nameof(UpdateAdminDetails))]
        public IActionResult UpdateAdminDetails(Administrator admin)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                if (admin.Id == 0)
                    return NotFound();

                _loggedInAdministratorFacade.UpdateAdminDetails(admin_token, admin);
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPut(nameof(UpdateCountryDetails))]
        public IActionResult UpdateCountryDetails(Country country)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                if (country.Id == 0)
                    return NotFound();

                _loggedInAdministratorFacade.UpdateCountryDetails(admin_token, country);
            }
            catch (RecordAlreadyExistsException)
            {
                return Conflict();
            }

            return NoContent();
        }

        [HttpDelete(nameof(RemoveAdmin))]
        public IActionResult RemoveAdmin(Administrator admin)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                _loggedInAdministratorFacade.RemoveAdmin(admin_token, admin);
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpDelete(nameof(RemoveCountry))]
        public IActionResult RemoveCountry(Country country)
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            try
            {
                _loggedInAdministratorFacade.RemoveCountry(admin_token, country);
            }
            catch (NotAllowedAdminActionException)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpGet(nameof(GetAllAdministrators))]
        public ActionResult<IList<Administrator>> GetAllAdministrators()
        {
            LoginToken<Administrator> admin_token = DesirializeToken();

            IList<Administrator> administrators = _loggedInAdministratorFacade.GetAllAdministrators(admin_token);
            if (administrators.Count == 0)
                return NoContent();

            return Ok(administrators);

        }
    }
}
