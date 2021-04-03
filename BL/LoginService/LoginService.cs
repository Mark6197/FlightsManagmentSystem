using BL.Exceptions;
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
        private IUserDAO _userDAO;

        public LoginService()
        {
            _adminDAO = new AdminDAOPGSQL();
            _customerDAO = new CustomerDAOPGSQL();
            _airlineDAO = new AirlineDAOPGSQL();
            _userDAO = new UserDAOPGSQL();
        }

        public bool TryLogin(string userName, string password, out ILoginToken token, out FacadeBase facade)
        {
            _logger.Info($"{userName} trying to login");
            if (userName == "admin" && password == "9999")
            {
                token = new LoginToken<Administrator>(new Administrator("Admin", "Admin", AdminLevel.Main_Admin, new User(userName, password, "admin@admin.com", UserRoles.Administrator)));
                facade = new LoggedInAdministratorFacade();
                _logger.Info($"{userName} succeeded to login as main administrator");
                return true;
            }

            try
            {
                User user = _userDAO.GetUserByUserNameAndPassword(userName,password);

                if (user == null)
                    throw new WrongCredentialsException();

                switch (user.UserRole)
                {
                    case UserRoles.Administrator:
                        Administrator administrator = _adminDAO.GetAdministratorByUserId(user.Id);
                        token = new LoginToken<Administrator>(administrator);
                        facade = new LoggedInAdministratorFacade();
                        break;
                    case UserRoles.Airline_Company:
                        AirlineCompany airlineCompany = _airlineDAO.GetAirlineCompanyByUserId(user.Id);
                        token = new LoginToken<AirlineCompany>(airlineCompany);
                        facade = new LoggedInAirlineFacade();
                        break;
                    case UserRoles.Customer:
                        Customer customer = _customerDAO.GetCustomerByUserId(user.Id);
                        token = new LoginToken<Customer>(customer);
                        facade = new LoggedInCustomerFacade();
                        break;
                    default://Will not happen
                        token = null;
                        facade = new AnonymousUserFacade();
                        break;
                }

                _logger.Info($"{userName} succeeded to login as {user.UserRole}");

                return true;
            }
            catch (WrongCredentialsException)
            {
                token = null;
                facade = new AnonymousUserFacade();
                _logger.Info($"{userName} failed to login");
                return false;
            }
        }

        public bool IsValidUserNameAndPassword(string username, string password)
        {
            //Administrator admin = _adminDAO.GetAdministratorByUsernameAndPassword(username, password);
            //if (admin != null)
            //    return true;

            //Customer customer = _customerDAO.GetCustomerByUsernameAndPassword(username, password);
            //if (customer != null)
            //    return true;

            //AirlineCompany airlineCompany = _airlineDAO.GetAirlineByUsernameAndPassword(username, password);
            //if (airlineCompany != null)
            //    return true;

            //return false;

            throw new NotImplementedException();
        }
    }
}
