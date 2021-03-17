using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL;
using Domain.Entities;
using System.Collections.Generic;
using System.Reflection;

namespace BL
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoggedInAdministratorFacade() : base()
        {
            _adminDAO = new AdminDAOPGSQL();
            _airlineDAO = new AirlineDAOPGSQL();
            _customerDAO = new CustomerDAOPGSQL();
        }

        public void CreateNewAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({admin})");

            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to add other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to add the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Add(admin);
            }
            catch (NotAllowedAdminActionException ex)
            {
                _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            }
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({airlineCompany})");

            _airlineDAO.Add(airlineCompany);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            _customerDAO.Add(customer);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _customerDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({admin})");

            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to remove the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Remove(admin);
            }
            catch (NotAllowedAdminActionException ex)
            {
                _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            }
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({airlineCompany})");

            try
            {
                if (token.User.Level == 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove airline companies. Admin's level is {token.User.Level}");

                _airlineDAO.Remove(airlineCompany);
            }
            catch (NotAllowedAdminActionException ex)
            {
                _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            }
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({customer})");

            try
            {
                if (token.User.Level == 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove customers. Admin's level is {token.User.Level}");

                _customerDAO.Remove(customer);
            }
            catch (NotAllowedAdminActionException ex)
            {
                _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            }
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({admin})");

            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to update other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 2)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to update the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Update(admin);
            }
            catch (NotAllowedAdminActionException ex)
            {
                _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            }
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({airlineCompany})");

            _airlineDAO.Update(airlineCompany);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");

        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({customer})");

            _customerDAO.Update(customer);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
        }
    }
}
