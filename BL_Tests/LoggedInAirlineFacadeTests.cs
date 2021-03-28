using BL;
using BL.Exceptions;
using BL.Interfaces;
using BL.LoginService;
using ConfigurationService;
using Domain.Entities;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BL_Tests
{
    [TestClass]
    public class LoggedInAirlineFacadeTests
    {
        private static readonly ILog my_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly FlightCenterSystem system = FlightCenterSystem.GetInstance();
        private LoggedInAirlineFacade airline_facade;
        private LoginToken<AirlineCompany> airline_token;

        [TestInitialize]
        public void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

            FlightsManagmentSystemConfig.Instance.Init();

            TestsDAOPGSQL.ClearDB();
            Init_Airline_Facade_Data();
        }

        private void Init_Airline_Facade_Data()
        {
            string username = "admin";
            string password = "9999";
            system.loginService.TryLogin(username, password, out ILoginToken admin_token, out FacadeBase admin_facade);
            LoggedInAdministratorFacade loggedInAdministratorFacade = admin_facade as LoggedInAdministratorFacade;
            LoginToken<Administrator> adminLoginToken = admin_token as LoginToken<Administrator>;
            int country_id = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[0]);
            int country_id2 = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[1]);
            int country_id3 = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[2]);
            long customer_id = loggedInAdministratorFacade.CreateNewCustomer(adminLoginToken, TestData.Get_Customers_Data()[0]);
            AirlineCompany airlineCompany = TestData.Get_AirlineCompanies_Data()[0];
            airlineCompany.CountryId = country_id;
            loggedInAdministratorFacade.CreateNewAirlineCompany(adminLoginToken, airlineCompany);
            Login();
            Flight flight = TestData.Get_Flights_Data()[3];
            Flight flight2 = TestData.Get_Flights_Data()[4];
            flight.AirlineCompany = airline_token.User;
            flight2.AirlineCompany = airline_token.User;
            long flight_id = airline_facade.CreateFlight(airline_token, flight);
            long flight_id2 = airline_facade.CreateFlight(airline_token, flight2);
            flight.Id = flight_id;
            flight2.Id = flight_id2;

            system.loginService.TryLogin(TestData.Get_Customers_Data()[0].User.UserName, TestData.Get_Customers_Data()[0].User.Password, out ILoginToken customer_token, out FacadeBase customer_facade);
            LoggedInCustomerFacade loggedInCustomerFacade = customer_facade as LoggedInCustomerFacade;
            LoginToken<Customer> customerLoginToken = customer_token as LoginToken<Customer>;
            //loggedInCustomerFacade.PurchaseTicket(customerLoginToken, flight);
            //loggedInCustomerFacade.PurchaseTicket(customerLoginToken, flight2);
        }

        private void Login()
        {
            system.loginService.TryLogin(TestData.Get_AirlineCompanies_Data()[0].User.UserName, TestData.Get_AirlineCompanies_Data()[0].User.Password, out ILoginToken token, out FacadeBase facade);
            airline_token = token as LoginToken<AirlineCompany>;
            airline_facade = facade as LoggedInAirlineFacade;
        }

        [TestMethod]
        public void Create_And_Get_New_Flight()
        {
            Flight demi_flight = TestData.Get_Flights_Data()[0];
            demi_flight.AirlineCompany = airline_token.User;
            long flight_id = airline_facade.CreateFlight(airline_token, demi_flight);
            Assert.AreEqual(flight_id, 3);
            demi_flight.Id = flight_id;
            Flight flight_from_db = airline_facade.GetFlightById((int)flight_id);
            flight_from_db.AirlineCompany.User = airline_token.User.User;//this line is added due to the sp dont return the user details

            TestData.CompareProps(flight_from_db, demi_flight);
        }

        [TestMethod]
        public void Create_And_Get_List_Of_New_Flight()
        {
            Flight[] data = TestData.Get_Flights_Data();
            Flight[] demi_flights = { data[0], data[1], data[2] };
            demi_flights[0].AirlineCompany = airline_token.User;
            demi_flights[1].AirlineCompany = airline_token.User;
            demi_flights[2].AirlineCompany = airline_token.User;

            for (int i = 0; i < demi_flights.Length; i++)
            {
                long flight_id = airline_facade.CreateFlight(airline_token, demi_flights[i]);
                Assert.AreEqual(flight_id, i + 3);
                demi_flights[i].Id = flight_id;
            }

            IList<Flight> flights_from_db = airline_facade.GetAllFlights();
            Assert.AreEqual(demi_flights.Length+2, flights_from_db.Count);
            
            for (int i = 2; i < flights_from_db.Count; i++)
            {
                flights_from_db[i].AirlineCompany.User = airline_token.User.User;//this line is added due to the sp dont return the user details

                TestData.CompareProps(flights_from_db[i], demi_flights[i-2]);
            }
        }

        [TestMethod]
        public void Update_Flight()
        {
            Flight demi_flight = TestData.Get_Flights_Data()[0];
            demi_flight.AirlineCompany = airline_token.User;
            long flight_id = airline_facade.CreateFlight(airline_token, demi_flight);
            demi_flight.Id = flight_id;
            demi_flight.LandingTime = DateTime.Now.AddYears(1);
            demi_flight.LandingTime = DateTime.Now.AddYears(1).AddDays(1);
            demi_flight.OriginCountryId = 1;
            demi_flight.DestinationCountryId = 1;
            demi_flight.RemainingTickets = 0;

            airline_facade.UpdateFlight(airline_token, demi_flight);

            Flight flight_from_db = airline_facade.GetFlightById((int)flight_id);
            flight_from_db.AirlineCompany.User = airline_token.User.User;//this line is added due to the sp dont return the user details

            TestData.CompareProps(flight_from_db, demi_flight);
        }

        [TestMethod]//Need to create a test method what will check if flight can be removed also when there are tickets associated with it
        public void Remove_Flight()
        {
            Flight demi_flight = TestData.Get_Flights_Data()[0];
            demi_flight.AirlineCompany = airline_token.User;
            long flight_id = airline_facade.CreateFlight(airline_token, demi_flight);
            demi_flight.Id = flight_id;

            airline_facade.CancelFlight(airline_token, demi_flight);

            Assert.AreEqual(airline_facade.GetAllFlights(airline_token).Count, 2);
        }

        [TestMethod]
        public void Change_Password()
        {
            string new_password = "new_pass";
            airline_facade.ChangeMyPassword(airline_token, "elalelal", new_password);
            AirlineCompany airlineCompany = airline_facade.GetAirlineCompanyById(airline_token.User.Id);
            Assert.AreEqual(airlineCompany.User.Password, new_password);
        }

        [TestMethod]
        public void Change_Password_With_Wrong_Old_Password()
        {
            string new_password = "new_pass";
            Assert.ThrowsException<WrongPasswordException>(() => airline_facade.ChangeMyPassword(airline_token, "wrongwrong", new_password));
        }

        [TestMethod]
        public void Change_Password_With_Same_Password()
        {
            Assert.ThrowsException<WrongPasswordException>(() => airline_facade.ChangeMyPassword(airline_token, "elalelal", "elalelal"));
        }

        [TestMethod]
        public void Change_Airline_Details()
        {
            AirlineCompany airlineCompany = airline_token.User;
            airlineCompany.Name = "Changed Name";
            airlineCompany.CountryId = 3;

            airline_facade.MofidyAirlineDetails(airline_token, airlineCompany);

            AirlineCompany airline_from_db = airline_facade.GetAirlineCompanyById(airlineCompany.Id);

            TestData.CompareProps(airlineCompany, airline_from_db);
        }
    }
}
