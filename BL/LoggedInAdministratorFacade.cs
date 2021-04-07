﻿using BL.Exceptions;
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
        }

        public int CreateNewAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            int result = 0;

            result = Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin || token.User.Level == AdminLevel.Mid_Level_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to add other admin. Admin's level is {token.User.Level}");

                if (token.User.Level != AdminLevel.Main_Admin && (admin.Level == AdminLevel.Main_Admin || admin.Level == AdminLevel.Senior_Admin))
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to add new admin at level {admin.Level}. Admin's level is {token.User.Level}");


                long user_id = _userDAO.Add(admin.User);

                admin.User.Id = user_id;
                result = (int)_adminDAO.Add(admin);

                return result;
            }, new { Token = token, Administrator = admin }, _logger);

            return result;
        }

        public long CreateNewAirlineCompany(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            long result = 0;

            result = Execute(() =>
            {
                long user_id = _userDAO.Add(airlineCompany.User);

                airlineCompany.User.Id = user_id;
                result = _airlineDAO.Add(airlineCompany);

                return result;
            }, new { Token = token, AirlineCompany = airlineCompany }, _logger);

            return result;
        }

        public int CreateNewCountry(LoginToken<Administrator> token, Country country)//maybe add validation only admin level 4+ can use it
        {
            int result = 0;

            result = Execute(() => (int)_countryDAO.Add(country), new { Token = token, Country = country }, _logger);

            return result;
        }

        public long CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            long result = 0;

            result = Execute(() =>
            {
                long user_id = _userDAO.Add(customer.User);

                customer.User.Id = user_id;
                result = _customerDAO.Add(customer);

                return result;
            }, new { Token = token, Customer = customer }, _logger);

            return result;
        }

        public Administrator GetAdminById(LoginToken<Administrator> token, int id)
        {
            Administrator result = null;

            result = Execute(() => _adminDAO.Get(id), new { Token = token, Id = id }, _logger);

            return result;
        }

        public IList<Administrator> GetAllAdministrators(LoginToken<Administrator> token)
        //might want to validate that only admin level 3+ can get all admins or maybe admin level 2 will get all admins level 2-
        {
            IList<Administrator> result = null;

            result = Execute(() => _adminDAO.GetAll(), new { Token = token }, _logger);

            return result;
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            IList<Customer> result = null;

            result = Execute(() => _customerDAO.GetAll(), new { Token = token }, _logger);

            return result;
        }

        public Customer GetCustomerById(LoginToken<Administrator> token, long id)
        {
            Customer result = null;

            result = Execute(() => _customerDAO.Get((int)id), new { Token = token, Id = id }, _logger);

            return result;
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin || token.User.Level == AdminLevel.Mid_Level_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove other admins. Admin's level is {token.User.Level}");

                if (admin.Level == AdminLevel.Senior_Admin || admin.Level == AdminLevel.Main_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} (level: {token.User.Level}) now allowed to remove the following admin: {admin.User.UserName} (level {admin.Level})");

                _adminDAO.Remove(admin);
                _userDAO.Remove(admin.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid 
            }, new { Token = token, Administrator = admin }, _logger);
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove airline companies. Admin's level is {token.User.Level}");

                IList<Ticket> tickets = _ticketDAO.GetTicketsByAirlineCompany(airlineCompany);
                if (tickets.Count > 0)
                    foreach (var ticket in tickets)
                    {
                        _flightsTicketsHistoryDAO.Add(ticket, TicketStatus.Cancelled_By_Administrator);
                        _ticketDAO.Remove(ticket);
                    }

                IList<Flight> flights = _flightDAO.GetFlightsByAirlineCompany(airlineCompany);
                if (flights.Count > 0)
                    foreach (var flight in flights)
                    {
                        _flightsTicketsHistoryDAO.Add(flight, FlightStatus.Cancelled_By_Administrator);
                        _flightDAO.Remove(flight);
                    }

                _airlineDAO.Remove(airlineCompany);
                _userDAO.Remove(airlineCompany.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid
            }, new { Token = token, AirlineCompany = airlineCompany }, _logger);
        }

        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove countries. Admin's level is {token.User.Level}");

                _countryDAO.Remove(country);
            }, new { Token = token, Country = country }, _logger);
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to remove customers. Admin's level is {token.User.Level}");


                IList<Ticket> tickets = _ticketDAO.GetTicketsByCustomer(customer);

                if (tickets.Count > 0)
                    foreach (var ticket in tickets)
                    {
                        _flightsTicketsHistoryDAO.Add(ticket, TicketStatus.Cancelled_By_Administrator);

                        _ticketDAO.Remove(ticket);
                    }

                _customerDAO.Remove(customer);
                _userDAO.Remove(customer.User);//not sure if this is a proper behavior, wen adding and removing airline/customer/admin is also add or remove user but when editing it only edits the userid

            }, new { Token = token, Customer = customer }, _logger);
        }

        public void UpdateAdminDetails(LoginToken<Administrator> token, Administrator admin)
        {
            Execute(() =>
            {
                if (token.User.Level == AdminLevel.Junior_Admin || token.User.Level == AdminLevel.Mid_Level_Admin)
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to update other admins. Admin's level is {token.User.Level}");

                if (token.User.Level != AdminLevel.Main_Admin && (admin.Level == AdminLevel.Main_Admin || admin.Level == AdminLevel.Senior_Admin))
                    throw new NotAllowedAdminActionException($"Admin {token.User.User.UserName} now allowed to update admin at level {admin.Level}. Admin's level is {token.User.Level}");

                _adminDAO.Update(admin);
            }, new { Token = token, Administrator = admin }, _logger);
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            Execute(() => _airlineDAO.Update(airlineCompany), new { Token = token, AirlineCompany = airlineCompany }, _logger);
        }

        public void UpdateCountryDetails(LoginToken<Administrator> token, Country country)
        {
            Execute(() => _countryDAO.Update(country), new { Token = token, Country = country }, _logger);
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            Execute(() => _customerDAO.Update(customer), new { Token = token, Customer = customer }, _logger);
        }
    }
}
