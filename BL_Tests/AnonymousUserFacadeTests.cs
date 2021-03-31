using BL;
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
    public class AnonymousUserFacadeTests
    {

        private static readonly ILog my_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly FlightCenterSystem system = FlightCenterSystem.GetInstance();
        private AnonymousUserFacade anonymous_facade;

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            TestsDAOPGSQL.ClearDB();
        }

        [TestInitialize]
        public void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

            FlightsManagmentSystemConfig.Instance.Init("FlightsManagmentSystemTests.Config.json");

            TestsDAOPGSQL.ClearDB();
            Init_Anonymous_Data();
            anonymous_facade = system.GetFacade<AnonymousUserFacade>();
        }

        private void Init_Anonymous_Data()
        {
            TestsDAOPGSQL.ClearDB();
            string username = "admin";
            string password = "9999";
            system.TryLogin(username, password, out ILoginToken admin_token, out FacadeBase admin_facade);
            LoggedInAdministratorFacade loggedInAdministratorFacade = admin_facade as LoggedInAdministratorFacade;
            LoginToken<Administrator> adminLoginToken = admin_token as LoginToken<Administrator>;
            int country_id = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[0]);
            int country_id2 = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[1]);
            int country_id3 = loggedInAdministratorFacade.CreateNewCountry(adminLoginToken, TestData.Get_Countries_Data()[2]);
            long customer_id = loggedInAdministratorFacade.CreateNewCustomer(adminLoginToken, TestData.Get_Customers_Data()[0]);
            AirlineCompany airlineCompany = TestData.Get_AirlineCompanies_Data()[0];
            airlineCompany.CountryId = country_id;
            loggedInAdministratorFacade.CreateNewAirlineCompany(adminLoginToken, airlineCompany);
            system.TryLogin(TestData.Get_AirlineCompanies_Data()[0].User.UserName, TestData.Get_AirlineCompanies_Data()[0].User.Password, out ILoginToken token, out FacadeBase facade);
            LoginToken<AirlineCompany> airline_token = token as LoginToken<AirlineCompany>;
            LoggedInAirlineFacade airline_facade = facade as LoggedInAirlineFacade;
            Flight flight = TestData.Get_Flights_Data_For_Anonymous_Tests()[0];
            Flight flight2 = TestData.Get_Flights_Data_For_Anonymous_Tests()[1];
            Flight flight3 = TestData.Get_Flights_Data_For_Anonymous_Tests()[2];
            Flight flight4 = TestData.Get_Flights_Data_For_Anonymous_Tests()[3];
            Flight flight5 = TestData.Get_Flights_Data_For_Anonymous_Tests()[4];
            Flight flight6 = TestData.Get_Flights_Data_For_Anonymous_Tests()[5];
            flight.AirlineCompany = airline_token.User;
            flight2.AirlineCompany = airline_token.User;
            flight3.AirlineCompany = airline_token.User;
            flight4.AirlineCompany = airline_token.User;
            flight5.AirlineCompany = airline_token.User;
            flight6.AirlineCompany = airline_token.User;
            long flight_id = airline_facade.CreateFlight(airline_token, flight);
            long flight_id2 = airline_facade.CreateFlight(airline_token, flight2);
            long flight_id3 = airline_facade.CreateFlight(airline_token, flight3);
            long flight_id4 = airline_facade.CreateFlight(airline_token, flight4);
            long flight_id5 = airline_facade.CreateFlight(airline_token, flight5);
            long flight_id6 = airline_facade.CreateFlight(airline_token, flight6);
            flight.Id = flight_id;
            flight2.Id = flight_id2;
            flight3.Id = flight_id3;
            flight4.Id = flight_id4;
            flight5.Id = flight_id5;
            flight6.Id = flight_id6;
        }

        [TestMethod]
        public void Get_All_Flights_By_Origin_Country()
        {
            Assert.AreEqual(anonymous_facade.GetFlightsByOriginCountry(1).Count, 3);
            Assert.AreEqual(anonymous_facade.GetFlightsByOriginCountry(2).Count, 2);
            Assert.AreEqual(anonymous_facade.GetFlightsByOriginCountry(3).Count, 1);
        }


        [TestMethod]
        public void Get_All_Flights_By_Destination_Country()
        {
            Assert.AreEqual(anonymous_facade.GetFlightsByDestinationCountry(1).Count, 0);
            Assert.AreEqual(anonymous_facade.GetFlightsByDestinationCountry(2).Count, 4);
            Assert.AreEqual(anonymous_facade.GetFlightsByDestinationCountry(3).Count, 2);
        }

        [TestMethod]
        public void Get_All_Flighst_By_Departure_Date()
        {
            Assert.AreEqual(anonymous_facade.GetFlightsByDepatrureDate(DateTime.Now).Count, 0);
            Assert.AreEqual(anonymous_facade.GetFlightsByDepatrureDate(DateTime.Now.AddDays(1)).Count, 3);
            Assert.AreEqual(anonymous_facade.GetFlightsByDepatrureDate(DateTime.Now.AddDays(2)).Count, 1);
            Assert.AreEqual(anonymous_facade.GetFlightsByDepatrureDate(DateTime.Now.AddDays(3)).Count, 1);
            Assert.AreEqual(anonymous_facade.GetFlightsByDepatrureDate(DateTime.Now.AddDays(4)).Count, 1);
        }

        [TestMethod]
        public void Get_All_Flights_By_Landing_Date()
        {
            Assert.AreEqual(anonymous_facade.GetFlightsByLandingDate(DateTime.Now.AddDays(2)).Count, 2);
            Assert.AreEqual(anonymous_facade.GetFlightsByLandingDate(DateTime.Now.AddDays(3)).Count, 2);
            Assert.AreEqual(anonymous_facade.GetFlightsByLandingDate(DateTime.Now.AddDays(4)).Count, 0);
            Assert.AreEqual(anonymous_facade.GetFlightsByLandingDate(DateTime.Now.AddDays(5)).Count, 2);
        }

        [TestMethod]
        public void Get_All_Flights_Vacancy()
        {
            Dictionary<Flight, int> flight_vacancy = anonymous_facade.GetAllFlightsVacancy();
            Assert.AreEqual(flight_vacancy.Count, 6);

            for (int i = 0; i < flight_vacancy.Count; i++)
            {
                Flight flight = TestData.Get_Flights_Data_For_Anonymous_Tests()[i];
                flight.Id = i + 1;

                Assert.AreEqual(flight.RemainingTickets, flight_vacancy[flight]);
            }
        }
    }
}
