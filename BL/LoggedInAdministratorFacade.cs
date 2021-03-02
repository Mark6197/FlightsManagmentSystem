using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using Domain.Entities;
using System.Collections.Generic;

namespace BL
{
    class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        public void CreateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to add other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to add the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Add(admin);
            }
            catch (NotAllowedAdminActionException)
            {
                throw;
            }
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _airlineDAO.Add(airlineCompany);
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            _customerDAO.Add(customer);
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            return _customerDAO.GetAll();
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to remove the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Remove(admin);
            }
            catch (NotAllowedAdminActionException)
            {
                throw;
            }
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            try
            {
                if (token.User.Level == 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove airline companies. Admin's level is {token.User.Level}");

                _airlineDAO.Remove(airlineCompany);
            }
            catch (NotAllowedAdminActionException)
            {
                throw;
            }
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            try
            {
                if (token.User.Level == 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove customers. Admin's level is {token.User.Level}");

                _customerDAO.Remove(customer);
            }
            catch (NotAllowedAdminActionException)
            {
                throw;
            }
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to update other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to update the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Update(admin);
            }
            catch (NotAllowedAdminActionException)
            {
                throw;
            }
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _airlineDAO.Update(airlineCompany);
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            _customerDAO.Update(customer);
        }
    }
}
