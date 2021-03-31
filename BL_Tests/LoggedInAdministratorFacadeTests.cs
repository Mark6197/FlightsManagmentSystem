using BL;
using BL.Exceptions;
using BL.LoginService;
using ConfigurationService;
using Domain.Entities;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BL_Tests
{
    [TestClass]
    public class LoggedInAdministratorFacadeTests
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly FlightCenterSystem system = FlightCenterSystem.GetInstance();
        private LoggedInAdministratorFacade administrator_facade;
        private LoginToken<Administrator> administrator_token;
        private LoggedInAdministratorFacade administrator_level_one_facade;
        private LoginToken<Administrator> administrator_level_one_token;


        [TestInitialize]
        public void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

            FlightsManagmentSystemConfig.Instance.Init();

            TestsDAOPGSQL.ClearDB();
            Login();
        }

        private void Login()
        {
            string username = "admin";
            string password = "9999";
            system.TryLogin(username, password, out ILoginToken token, out FacadeBase facade);
            administrator_token = token as LoginToken<Administrator>;
            administrator_facade = facade as LoggedInAdministratorFacade;
        }
        private void Init_Admin_Level_One_And_Login()
        {
            Administrator admin_level_one = TestData.Get_Administrators_Data()[0];
            int admin_level_one_id = administrator_facade.CreateNewAdmin(administrator_token, admin_level_one);
            system.TryLogin(admin_level_one.User.UserName, admin_level_one.User.Password, out ILoginToken token, out FacadeBase facade);
            administrator_level_one_token = token as LoginToken<Administrator>;
            administrator_level_one_facade = facade as LoggedInAdministratorFacade;
        }

        [TestMethod]
        public void Valid_Main_Administrator_Login()
        {
            string username = "admin";
            string password = "9999";
            bool result = system.TryLogin(username, password, out ILoginToken token, out FacadeBase facade);
            Assert.IsTrue(result);
            Assert.AreEqual(0, administrator_token.User.Id);
            Assert.AreEqual("Admin", administrator_token.User.FirstName);
            Assert.AreEqual("Admin", administrator_token.User.LastName);
            Assert.AreEqual(4, administrator_token.User.Level);
            Assert.AreEqual(0, administrator_token.User.User.Id);
            Assert.AreEqual(username, administrator_token.User.User.UserName);
            Assert.AreEqual(password, administrator_token.User.User.Password);
            Assert.AreEqual("admin@admin.com", administrator_token.User.User.Email);
            Assert.AreEqual(UserRoles.Administrator, administrator_token.User.User.UserRole);
        }

        [TestMethod]
        public void Invalid_Administrator_Login()
        {
            string username = "adminn";
            string password = "9999";
            bool result = system.TryLogin(username, password, out ILoginToken token, out FacadeBase anonymous_facade);
            Assert.IsFalse(result);
            Assert.IsNull(token);
            Assert.IsInstanceOfType(anonymous_facade, typeof(AnonymousUserFacade));

            username = "admin";
            password = "99999";
            bool result2 = system.TryLogin(username, password, out ILoginToken token2, out FacadeBase anonymous_facade2);
            Assert.IsFalse(result2);
            Assert.IsNull(token2);
            Assert.IsInstanceOfType(anonymous_facade2, typeof(AnonymousUserFacade));
        }

        [TestMethod]
        public void Create_And_Get_New_Administrator()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_administrator);
            Assert.AreEqual(admin_id, 1);
            demi_administrator.Id = admin_id;
            Administrator administrator_from_db = administrator_facade.GetAdminById(administrator_token, admin_id);

            TestData.CompareProps(administrator_from_db, demi_administrator);
        }

        [TestMethod]
        public void Create_New_Administrator_Using_Level_One_Admin()
        {
            Init_Admin_Level_One_And_Login();
            Administrator demi_administrator = TestData.Get_Administrators_Data()[1];
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_level_one_facade.CreateNewAdmin(administrator_level_one_token, demi_administrator));
        }

        [TestMethod]
        public void Create_New_Administrator_With_Level_Four()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            demi_administrator.Level = 4;
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_facade.CreateNewAdmin(administrator_token, demi_administrator));
        }


        [TestMethod]
        public void Create_New_Administrator_With_Level_Zero()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            demi_administrator.Level = 0;
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_facade.CreateNewAdmin(administrator_token, demi_administrator));
        }

        [TestMethod]
        public void Create_And_Get_List_Of_Administrators()
        {
            Administrator[] data = TestData.Get_Administrators_Data();
            Administrator[] demi_administrators = { data[0], data[1], data[2] };
            for (int i = 0; i < demi_administrators.Length; i++)
            {
                int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_administrators[i]);
                Assert.AreEqual(admin_id, i + 1);
                demi_administrators[i].Id = admin_id;
            }

            IList<Administrator> administrators_from_db = administrator_facade.GetAllAdministrators(administrator_token);
            Assert.AreEqual(demi_administrators.Length, administrators_from_db.Count);
            for (int i = 0; i < administrators_from_db.Count; i++)
            {
                TestData.CompareProps(administrators_from_db[i], demi_administrators[i]);
            }
        }

        [TestMethod]
        public void Get_Administrator_That_Not_Exists()
        {
            Administrator administrator_from_db = administrator_facade.GetAdminById(administrator_token, 1);

            Assert.IsNull(administrator_from_db);
        }

        [TestMethod]
        public void Create_Two_Administrators_With_Same_Username()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            Administrator demi_administrator_with_same_username = TestData.Get_Administrators_Data()[3];

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_administrator);
            Assert.AreEqual(admin_id, 1);

            Assert.ThrowsException<PostgresException>(() => administrator_facade.CreateNewAdmin(administrator_token, demi_administrator_with_same_username));
        }

        [TestMethod]
        public void Create_Two_Administrators_With_Same_Email()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            Administrator demi_administrator_with_same_email = TestData.Get_Administrators_Data()[4];

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_administrator);
            Assert.AreEqual(admin_id, 1);

            Assert.ThrowsException<PostgresException>(() => administrator_facade.CreateNewAdmin(administrator_token, demi_administrator_with_same_email));
        }

        [TestMethod]
        public void Create_Two_Administrators_With_Same_User_Id()
        {
            Administrator demi_administrator = TestData.Get_Administrators_Data()[0];
            Administrator demi_administrator_with_same_user_id = TestData.Get_Administrators_Data()[1];
            demi_administrator_with_same_user_id.User.Id = 1;

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_administrator);
            Assert.AreEqual(admin_id, 1);

            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_facade.CreateNewAdmin(administrator_token, demi_administrator_with_same_user_id));
        }

        [TestMethod]
        public void Create_And_Get_New_Country()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];
            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);
            Assert.AreEqual(country_id, 1);
            demi_country.Id = country_id;
            Country country_from_db = administrator_facade.GetCountryById(country_id);

            TestData.CompareProps(country_from_db, demi_country);
        }

        [TestMethod]
        public void Create_And_Get_List_Of_New_Countries()
        {
            Country[] data = TestData.Get_Countries_Data();
            Country[] demi_countries = { data[0], data[1], data[2], data[3], data[4] };
            for (int i = 0; i < demi_countries.Length; i++)
            {
                int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_countries[i]);
                Assert.AreEqual(country_id, i + 1);
                demi_countries[i].Id = country_id;
            }

            IList<Country> countries_from_db = administrator_facade.GetAllCountries();
            Assert.AreEqual(demi_countries.Length, countries_from_db.Count);
            for (int i = 0; i < countries_from_db.Count; i++)
            {
                TestData.CompareProps(countries_from_db[i], demi_countries[i]);
            }
        }

        [TestMethod]
        public void Create_Two_Countries_With_Same_Name()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];

            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);
            Assert.AreEqual(country_id, 1);

            Assert.ThrowsException<PostgresException>(() => administrator_facade.CreateNewCountry(administrator_token, demi_country));
        }

        [TestMethod]
        public void Get_Country_That_Not_Exists()
        {
            Country country_from_db = administrator_facade.GetCountryById(1);

            Assert.IsNull(country_from_db);
        }

        [TestMethod]
        public void Create_And_Get_New_Customer()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            Assert.AreEqual(customer_id, 1);
            demi_customer.Id = customer_id;
            Customer customer_from_db = administrator_facade.GetCustomerById(administrator_token, customer_id);

            TestData.CompareProps(customer_from_db, demi_customer);
        }

        [TestMethod]
        public void Create_And_Get_List_Of_New_Customers()
        {
            Customer[] data = TestData.Get_Customers_Data();
            Customer[] demi_customers = { data[0], data[1], data[2] };
            for (int i = 0; i < demi_customers.Length; i++)
            {
                long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customers[i]);
                Assert.AreEqual(customer_id, i + 1);
                demi_customers[i].Id = customer_id;
            }

            IList<Customer> customers_from_db = administrator_facade.GetAllCustomers(administrator_token);
            Assert.AreEqual(demi_customers.Length, customers_from_db.Count);
            for (int i = 0; i < customers_from_db.Count; i++)
            {
                TestData.CompareProps(customers_from_db[i], demi_customers[i]);
            }
        }

        [TestMethod]
        public void Get_Customer_That_Not_Exists()
        {
            Customer customer_from_db = administrator_facade.GetCustomerById(administrator_token, 1);

            Assert.IsNull(customer_from_db);
        }

        [TestMethod]
        public void Create_Two_Customers_With_Same_User_Id()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            Customer demi_customer_with_same_user_id = TestData.Get_Customers_Data()[1];
            demi_customer_with_same_user_id.User.Id = 1;

            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            Assert.AreEqual(customer_id, 1);

            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_facade.CreateNewCustomer(administrator_token, demi_customer_with_same_user_id));
        }

        [TestMethod]
        public void Create_Two_Customers_With_Same_Phone_Number()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            Customer demi_customer_with_same_phone_number = TestData.Get_Customers_Data()[3];

            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            Assert.AreEqual(customer_id, 1);

            Assert.ThrowsException<PostgresException>(() => administrator_facade.CreateNewCustomer(administrator_token, demi_customer_with_same_phone_number));
        }


        [TestMethod]
        public void Create_Two_Customers_With_Same_Credit_Card()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            Customer demi_customer_with_same_credit_card = TestData.Get_Customers_Data()[4];

            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            Assert.AreEqual(customer_id, 1);

            Assert.ThrowsException<PostgresException>(() => administrator_facade.CreateNewCustomer(administrator_token, demi_customer_with_same_credit_card));
        }


        [TestMethod]
        public void Create_And_Get_New_Airline()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];
            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);
            Assert.AreEqual(airline_company_id, 1);
            demi_airline_company.Id = airline_company_id;
            AirlineCompany airline_company_from_db = administrator_facade.GetAirlineCompanyById(airline_company_id);

            TestData.CompareProps(airline_company_from_db, demi_airline_company);
        }

        [TestMethod]
        public void Create_And_Get_List_Of_New_Airlines()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);
            int country_id2 = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[2]);
            int country_id3 = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[5]);


            AirlineCompany[] data = TestData.Get_AirlineCompanies_Data();
            AirlineCompany[] demi_airline_companies = { data[0], data[1], data[2] };
            demi_airline_companies[0].CountryId = country_id;
            demi_airline_companies[1].CountryId = country_id2;
            demi_airline_companies[2].CountryId = country_id3;
            for (int i = 0; i < demi_airline_companies.Length; i++)
            {
                long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_companies[i]);
                Assert.AreEqual(airline_company_id, i + 1);
                demi_airline_companies[i].Id = airline_company_id;
            }

            IList<AirlineCompany> airline_companies_from_db = administrator_facade.GetAllAirlineCompanies();
            Assert.AreEqual(demi_airline_companies.Length, airline_companies_from_db.Count);
            for (int i = 0; i < airline_companies_from_db.Count; i++)
            {
                TestData.CompareProps(airline_companies_from_db[i], demi_airline_companies[i]);
            }
        }

        [TestMethod]
        public void Get_Airline_That_Not_Exists()
        {
            AirlineCompany airline_company_from_db = administrator_facade.GetAirlineCompanyById(1);

            Assert.IsNull(airline_company_from_db);
        }

        [TestMethod]
        public void Update_Airline()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);
            int country_id2 = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[1]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];

            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);
            demi_airline_company.Id = airline_company_id;
            demi_airline_company.Name = "Changed name";
            demi_airline_company.CountryId = country_id2;

            administrator_facade.UpdateAirlineDetails(administrator_token, demi_airline_company);

            AirlineCompany updated_airline_company = administrator_facade.GetAirlineCompanyById(airline_company_id);

            TestData.CompareProps(demi_airline_company, updated_airline_company);
        }

        [TestMethod]
        public void Update_Airline_With_CountryId_That_Not_Exists()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];

            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);

            demi_airline_company.Id = airline_company_id;
            demi_airline_company.CountryId = 99;

            Assert.ThrowsException<PostgresException>(() => administrator_facade.UpdateAirlineDetails(administrator_token, demi_airline_company));
        }


        [TestMethod]
        public void Update_Airline_With_UserId_That_Not_Exists()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];

            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);

            demi_airline_company.Id = airline_company_id;
            demi_airline_company.User.Id = 99;

            Assert.ThrowsException<PostgresException>(() => administrator_facade.UpdateAirlineDetails(administrator_token, demi_airline_company));
        }

        [TestMethod]
        public void Update_Admin()
        {
            Administrator demi_admin = TestData.Get_Administrators_Data()[0];

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_admin);
            demi_admin.Id = admin_id;
            demi_admin.FirstName = "Changed";
            demi_admin.LastName = "Name";
            demi_admin.Level = 2;

            administrator_facade.UpdateAdminDetails(administrator_token, demi_admin);

            Administrator updated_admin = administrator_facade.GetAdminById(administrator_token, admin_id);

            TestData.CompareProps(demi_admin, updated_admin);
        }

        [TestMethod]
        public void Update_Admin_Using_Level_One_Admin()
        {
            Administrator demi_admin = TestData.Get_Administrators_Data()[1];

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_admin);
            demi_admin.Id = admin_id;
            demi_admin.FirstName = "Changed";
            demi_admin.LastName = "Name";
            demi_admin.Level = 1;

            Init_Admin_Level_One_And_Login();
            Assert.ThrowsException<NotAllowedAdminActionException>(()=> administrator_level_one_facade.UpdateAdminDetails(administrator_level_one_token, demi_admin));
        }

        [TestMethod]
        public void Update_Admin_With_UserId_That_Not_Exists()
        {
            Administrator demi_admin = TestData.Get_Administrators_Data()[0];

            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_admin);

            demi_admin.Id = admin_id;
            demi_admin.User.Id = 99;

            Assert.ThrowsException<PostgresException>(() => administrator_facade.UpdateAdminDetails(administrator_token, demi_admin));
        }

        [TestMethod]
        public void Update_Customer()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];

            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            demi_customer.Id = customer_id;
            demi_customer.FirstName = "Changed";
            demi_customer.LastName = "Name";
            demi_customer.Address = "Changed Address";
            demi_customer.CreditCardNumber = "11111111111111111111111";
            demi_customer.PhoneNumber = "11111111111";

            administrator_facade.UpdateCustomerDetails(administrator_token, demi_customer);

            Customer updated_customer = administrator_facade.GetCustomerById(administrator_token, customer_id);

            TestData.CompareProps(demi_customer, updated_customer);
        }

        [TestMethod]
        public void Update_Customer_With_UserId_That_Not_Exists()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];

            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);
            demi_customer.Id = customer_id;
            demi_customer.User.Id = 99;

            Assert.ThrowsException<PostgresException>(() => administrator_facade.UpdateCustomerDetails(administrator_token, demi_customer));
        }

        [TestMethod]
        public void Update_Country()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];
            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);

            demi_country.Id = country_id;
            demi_country.Name = "Some other name";

            administrator_facade.UpdateCountryDetails(administrator_token, demi_country);

            Country updated_country = administrator_facade.GetCountryById(country_id);

            TestData.CompareProps(demi_country, updated_country);
        }

        [TestMethod]
        public void Update_Country_With_Same_Name()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];
            Country demi_country2 = TestData.Get_Countries_Data()[1];

            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);
            int country_id2 = administrator_facade.CreateNewCountry(administrator_token, demi_country2);

            demi_country2.Id = country_id2;
            demi_country2.Name = demi_country.Name;

            Assert.ThrowsException<PostgresException>(() => administrator_facade.UpdateCountryDetails(administrator_token, demi_country2));
        }

        [TestMethod]
        public void Remove_Country()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];
            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);

            demi_country.Id = country_id;

            administrator_facade.RemoveCountry(administrator_token, demi_country);

            Assert.AreEqual(administrator_facade.GetAllCountries().Count, 0);
        }

        [TestMethod]
        public void Remove_Country_Using_Level_One_Admin()
        {
            Country demi_country = TestData.Get_Countries_Data()[0];
            int country_id = administrator_facade.CreateNewCountry(administrator_token, demi_country);

            demi_country.Id = country_id;

            Init_Admin_Level_One_And_Login();
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_level_one_facade.RemoveCountry(administrator_level_one_token, demi_country));
        }

        [TestMethod]
        public void Remove_Customer()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);

            demi_customer.Id = customer_id;

            administrator_facade.RemoveCustomer(administrator_token, demi_customer);

            Assert.AreEqual(administrator_facade.GetAllCustomers(administrator_token).Count, 0);
        }

        [TestMethod]
        public void Remove_Customer_Using_Level_One_Admin()
        {
            Customer demi_customer = TestData.Get_Customers_Data()[0];
            long customer_id = administrator_facade.CreateNewCustomer(administrator_token, demi_customer);

            demi_customer.Id = customer_id;

            Init_Admin_Level_One_And_Login();
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_level_one_facade.RemoveCustomer(administrator_level_one_token, demi_customer));
        }

        [TestMethod]
        public void Remove_Admin()
        {
            Administrator demi_admin = TestData.Get_Administrators_Data()[0];
            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_admin);

            demi_admin.Id = admin_id;

            administrator_facade.RemoveAdmin(administrator_token, demi_admin);

            Assert.AreEqual(administrator_facade.GetAllAdministrators(administrator_token).Count, 0);
        }

        [TestMethod]
        public void Remove_Admin_Using_Level_One_Admin()
        {
            Administrator demi_admin = TestData.Get_Administrators_Data()[1];
            int admin_id = administrator_facade.CreateNewAdmin(administrator_token, demi_admin);

            demi_admin.Id = admin_id;

            Init_Admin_Level_One_And_Login();
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_level_one_facade.RemoveAdmin(administrator_level_one_token, demi_admin));
        }

        [TestMethod]
        public void Remove_Airline_Company()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];
            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);

            demi_airline_company.Id = airline_company_id;

            administrator_facade.RemoveAirline(administrator_token, demi_airline_company);

            Assert.AreEqual(administrator_facade.GetAllAirlineCompanies().Count, 0);
        }

        [TestMethod]
        public void Remove_Airline_Company_Using_Level_One_Admin()
        {
            int country_id = administrator_facade.CreateNewCountry(administrator_token, TestData.Get_Countries_Data()[0]);

            AirlineCompany demi_airline_company = TestData.Get_AirlineCompanies_Data()[0];
            demi_airline_company.CountryId = country_id;
            long airline_company_id = administrator_facade.CreateNewAirlineCompany(administrator_token, demi_airline_company);

            demi_airline_company.Id = airline_company_id;

            Init_Admin_Level_One_And_Login();
            Assert.ThrowsException<NotAllowedAdminActionException>(() => administrator_level_one_facade.RemoveAirline(administrator_level_one_token, demi_airline_company));
        }
    }
}
