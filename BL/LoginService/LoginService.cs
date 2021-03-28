using DAL;
using Domain.Entities;
using Domain.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BL.LoginService
{
    public class LoginService : ILoginService
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IAirlineDAO _airlineDAO;
        private ICustomerDAO _customerDAO;
        private IAdminDAO _adminDAO;

        public LoginService()
        {
            _adminDAO = new AdminDAOPGSQL();
            _customerDAO = new CustomerDAOPGSQL();
            _airlineDAO = new AirlineDAOPGSQL();
        }

        public bool TryLogin(string userName, string password, out ILoginToken token, out FacadeBase facade)
        {
            _logger.Info($"{userName} trying to login");
            try
            {
                if (userName == "admin" && password == "9999")
                {
                    token = new LoginToken<Administrator>(new Administrator("Admin", "Admin", 4, new User(userName, password, "admin@admin.com", UserRoles.Administrator)));
                    facade = new LoggedInAdministratorFacade();
                    _logger.Info($"{userName} succeeded to login as main administrator");
                    return true;
                }

                Administrator admin = _adminDAO.GetAdministratorByUsernameAndPassword(userName, password);
                if (admin != null)
                {
                    token = new LoginToken<Administrator>(admin);
                    facade = new LoggedInAdministratorFacade();
                    _logger.Info($"{userName} succeeded to login as administrator");
                    return true;
                }

                Customer customer = _customerDAO.GetCustomerByUsernameAndPassword(userName, password);
                if (customer != null)
                {
                    token = new LoginToken<Customer>(customer);
                    facade = new LoggedInCustomerFacade();
                    _logger.Info($"{userName} succeeded to login as customer");
                    return true;
                }

                AirlineCompany airlineCompany = _airlineDAO.GetAirlineByUsernameAndPassword(userName, password);
                if (airlineCompany != null)
                {
                    token = new LoginToken<AirlineCompany>(airlineCompany);
                    facade = new LoggedInAirlineFacade();
                    _logger.Info($"{userName} succeeded to login as airline company");
                    return true;
                }

                throw new WrongCredentialsException();
            }
            catch (WrongCredentialsException)
            {
                token = null;
                facade = new AnonymousUserFacade();
                _logger.Info($"{userName} failed to login");
                return false;
            }


        }
    }
}
