using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using DAL;
using Domain.Entities;
using Npgsql;
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
            _countryDAO = new CountryDAOPGSQL();
            _userDAO = new UserDAOPGSQL();
        }

        public int CreateNewAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({admin})");
            int admin_id = 0;
            try
            {
                if (token.User.Level < 3)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to add other admins. Admin's level is {token.User.Level}");

                if (admin.Level > 3 || admin.Level < 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to add the following admin: {admin.User.UserName} (level {admin.Level})");

                //Check if user role of user is indeed admin?
                if (admin.User.Id != 0)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) tried to add the following admin: {admin.User.UserName} (level {admin.Level}), The admin has foreign key to another user");

                //return 0;

                long user_id = _userDAO.Add(admin.User);
                //if (user_id == 0)
                //    return 0;

                admin.User.Id = user_id;
                admin_id = (int)_adminDAO.Add(admin);
                return admin_id;
            }
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //    return 0;
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {admin_id}");
            }
        }

        public long CreateNewAirlineCompany(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({airlineCompany})");

            if (airlineCompany.User.Id != 0)
                throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) tried to add the following airline company: {airlineCompany}, The airline company has foreign key to another user");

            long user_id = _userDAO.Add(airlineCompany.User);

            airlineCompany.User.Id = user_id;
            long airline_company_id = _airlineDAO.Add(airlineCompany);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {airline_company_id}");

            return airline_company_id;
        }

        public int CreateNewCountry(LoginToken<Administrator> token, Country country)//maybe add validation only admin level 4+ can use it
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            int country_id = (int)_countryDAO.Add(country);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {country_id}");

            return country_id;
        }

        public long CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            if (customer.User.Id != 0)
                throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) tried to add the following customer: {customer}, The customer has foreign key to another user");

            long user_id = _userDAO.Add(customer.User);

            customer.User.Id = user_id;
            long customer_id = _customerDAO.Add(customer);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {customer_id}");

            return customer_id;
        }

        public Administrator GetAdminById(LoginToken<Administrator> token, int id)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({id})");

            var result = _adminDAO.Get(id);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Administrator> GetAllAdministrators(LoginToken<Administrator> token)
        //might want to validate that only admin level 3+ can get all admins or maybe admin level 2 will get all admins level 2-
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _adminDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _customerDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public Customer GetCustomerById(LoginToken<Administrator> token, long id)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({id})");

            var result = _customerDAO.Get((int)id);

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
                _userDAO.Remove(admin.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid
            }
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
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
                _userDAO.Remove(airlineCompany.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid
            }
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({country})");

            try
            {
                if (token.User.Level == 1)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove countries. Admin's level is {token.User.Level}");

                _countryDAO.Remove(country);
            }
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
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
                _userDAO.Remove(customer.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid
            }
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
            finally
            {
                _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}");
            }
        }

        public void UpdateAdminDetails(LoginToken<Administrator> token, Administrator admin)
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
            //catch (NotAllowedAdminActionException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //}
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

        public void UpdateCountryDetails(LoginToken<Administrator> token, Country country)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({country})");

            _countryDAO.Update(country);

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
