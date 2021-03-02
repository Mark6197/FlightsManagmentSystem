using DAL;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.LoginService
{
    public class LoginService : ILoginService
    {
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
            try
            {
                if (userName == "admin" && password == "9999")
                {
                    token = new LoginToken<Administrator>(new Administrator("Admin", "Admin", 4, null));
                    facade = new LoggedInAdministratorFacade();
                    return true;
                }

                Administrator admin = _adminDAO.GetAdministratorByUsernameAndPassword(userName, password);
                if (admin != null)
                {
                    token = new LoginToken<Administrator>(admin);
                    facade = new LoggedInAdministratorFacade();
                    return true;
                }

                Customer customer = _customerDAO.GetCustomerByUsernameAndPassword(userName, password);
                if (admin != null)
                {
                    token = new LoginToken<Customer>(customer);
                    facade = new LoggedInCustomerFacade();
                    return true;
                }

                AirlineCompany airlineCompany = _airlineDAO.GetAirlineByUsernameAndPassword(userName, password);
                if (admin != null)
                {
                    token = new LoginToken<AirlineCompany>(airlineCompany);
                    facade = new LoggedInAirlineFacade();
                    return true;
                }

                throw new WrongCredentialsException();
            }
            catch (WrongCredentialsException)
            {
                token = null;
                facade = new AnonymousUserFacade();
                return false;
            }


        }
    }
}
