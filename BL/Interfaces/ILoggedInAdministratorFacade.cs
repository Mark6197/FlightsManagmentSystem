using BL.LoginService;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface ILoggedInAdministratorFacade
    {
        IList<Customer> GetAllCustomers(LoginToken<Administrator> token);
        void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany customer);
        void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void CreateNewCustomer(LoginToken<Administrator> token, Customer customer);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer);
        void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        void CreateAdmin(LoginToken<Administrator> token, Administrator admin);
        void UpdateAdmin(LoginToken<Administrator> token, Administrator admin);
        void RemoveAdmin(LoginToken<Administrator> token, Administrator admin);
    }
}
