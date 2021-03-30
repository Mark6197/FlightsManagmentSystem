﻿using BL;
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
using System.Text;

namespace BL_Tests
{
    [TestClass]
    public class LoggedInCustomerFacadeTests
    {
        private static readonly ILog my_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly FlightCenterSystem system = FlightCenterSystem.GetInstance();
        private LoggedInCustomerFacade customer_facade;
        private LoginToken<Customer> customer_token;

        [TestInitialize]
        public void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));

            FlightsManagmentSystemConfig.Instance.Init();

            TestsDAOPGSQL.ClearDB();
            Init_Customer_Facade_Data();
        }

        private void Init_Customer_Facade_Data()
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
            long customer_id2 = loggedInAdministratorFacade.CreateNewCustomer(adminLoginToken, TestData.Get_Customers_Data()[1]);

            AirlineCompany airlineCompany = TestData.Get_AirlineCompanies_Data()[0];
            airlineCompany.CountryId = country_id;
            loggedInAdministratorFacade.CreateNewAirlineCompany(adminLoginToken, airlineCompany);
            system.loginService.TryLogin(airlineCompany.User.UserName, airlineCompany.User.Password, out ILoginToken airline_token, out FacadeBase airline_facade);
            LoggedInAirlineFacade loggedInAirlineFacade = airline_facade as LoggedInAirlineFacade;
            LoginToken<AirlineCompany> airlineLoginToken = airline_token as LoginToken<AirlineCompany>;
            Flight flight = TestData.Get_Flights_Data()[3];
            Flight flight2 = TestData.Get_Flights_Data()[4];
            Flight flight3 = TestData.Get_Flights_Data()[5];
            flight.AirlineCompany = airlineLoginToken.User;
            flight2.AirlineCompany = airlineLoginToken.User;
            flight3.AirlineCompany = airlineLoginToken.User;
            long flight_id = loggedInAirlineFacade.CreateFlight(airlineLoginToken, flight);
            long flight_id2 = loggedInAirlineFacade.CreateFlight(airlineLoginToken, flight2);
            long flight_id3 = loggedInAirlineFacade.CreateFlight(airlineLoginToken, flight3);
            flight.Id = flight_id;
            flight2.Id = flight_id2;
            flight3.Id = flight_id3;

            system.loginService.TryLogin(TestData.Get_Customers_Data()[1].User.UserName, TestData.Get_Customers_Data()[1].User.Password, out ILoginToken token, out FacadeBase facade);
            LoginToken<Customer> cust_token = token as LoginToken<Customer>;
            LoggedInCustomerFacade cust_facade = facade as LoggedInCustomerFacade;
            cust_facade.PurchaseTicket(cust_token, flight);

            Login(TestData.Get_Customers_Data()[0].User.UserName, TestData.Get_Customers_Data()[0].User.Password);
        }


        private void Login(string user_name, string password)
        {
            system.loginService.TryLogin(user_name, password, out ILoginToken token, out FacadeBase facade);
            customer_token = token as LoginToken<Customer>;
            customer_facade = facade as LoggedInCustomerFacade;
        }

        [TestMethod]
        public void Purchase_And_Get_Ticket()
        {
            Flight flight = customer_facade.GetFlightById(1);
            int empty_seates_before_purchase = flight.RemainingTickets;
            Ticket ticket = customer_facade.PurchaseTicket(customer_token, flight);
            Assert.AreEqual(ticket.Id, 2);
            Ticket my_ticket = customer_facade.GetTicketById(customer_token, ticket.Id);

            TestData.CompareProps(ticket, my_ticket, true);

            flight = customer_facade.GetFlightById(1);
            int empty_seates_after_purchase = flight.RemainingTickets;
            Assert.AreEqual(empty_seates_before_purchase - 1, empty_seates_after_purchase);
        }

        [TestMethod]
        public void Purchase_And_Get_Two_Tickets_And_Two_Flights()
        {
            Flight flight = customer_facade.GetFlightById(1);
            Flight flight2 = customer_facade.GetFlightById(2);
            Ticket ticket = customer_facade.PurchaseTicket(customer_token, flight);
            Ticket ticket2 = customer_facade.PurchaseTicket(customer_token, flight2);
            Assert.AreEqual(ticket.Id, 2);
            Assert.AreEqual(ticket2.Id, 3);
            IList<Ticket> my_tickets = customer_facade.GetAllMyTickets(customer_token);
            Assert.AreEqual(my_tickets.Count, 2);

            TestData.CompareProps(my_tickets[0], ticket, true);
            TestData.CompareProps(my_tickets[1], ticket2, true);

            IList<Flight> my_flights = customer_facade.GetAllMyFlights(customer_token);
            Assert.AreEqual(my_flights[0], flight);
            Assert.AreEqual(my_flights[1], flight2);
        }

        [TestMethod]
        public void Purchase_Two_Tickets_For_The_Same_Flight()
        {
            Flight flight = customer_facade.GetFlightById(1);
            Ticket ticket = customer_facade.PurchaseTicket(customer_token, flight);
            Assert.AreEqual(ticket.Id, 2);
            Assert.ThrowsException<PostgresException>(() => customer_facade.PurchaseTicket(customer_token, flight));
        }

        [TestMethod]
        public void Purchase_Tickets_For_Empty_Flight()
        {
            Flight flight = customer_facade.GetFlightById(3);
            Assert.ThrowsException<TicketPurchaseFailedException>(() => customer_facade.PurchaseTicket(customer_token, flight));
        }


        [TestMethod]
        public void Cancel_Ticket()
        {
            Flight flight = customer_facade.GetFlightById(1);
            Ticket ticket = customer_facade.PurchaseTicket(customer_token, flight);
            Assert.AreEqual(ticket.Id, 2);

            flight = customer_facade.GetFlightById(1);
            int empty_seates_before_cancellation = flight.RemainingTickets;

            customer_facade.CancelTicket(customer_token, ticket);
            Ticket ticket_from_db = customer_facade.GetTicketById(customer_token, ticket.Id);
            Assert.AreEqual(ticket_from_db, null);

            flight = customer_facade.GetFlightById(1);
            int empty_seates_after_cancellation = flight.RemainingTickets;
            Assert.AreEqual(empty_seates_before_cancellation + 1, empty_seates_after_cancellation);
        }

        [TestMethod]
        public void Get_Ticket_Of_Another_Customer()
        {
            Assert.ThrowsException<NotAllowedCustomerActionException>(() => customer_facade.GetTicketById(customer_token, 1));
        }

        [TestMethod]
        public void Cancel_Ticket_Of_Another_Customer()
        {
            Assert.ThrowsException<NotAllowedCustomerActionException>(() => customer_facade.CancelTicket(customer_token, new Ticket(new Flight(), new Customer { Id = 2 }, 1)));
        }
    }
}
