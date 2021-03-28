using BL;
using BL.Interfaces;
using BL.LoginService;
using ConfigurationService;
using DAL;
using Domain.Entities;
using Domain.Interfaces;
using log4net;
using log4net.Config;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Test
{
    class Program
    {
        private static readonly ILog my_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("Log4Net.config"));


            //my_logger.Info("Test again");
            if (args.Length == 1)
            {
                FlightsManagmentSystemConfig.Instance.Init(args[0]);
            }
            else
            {
                FlightsManagmentSystemConfig.Instance.Init();
            }
            DbConnectionPool.Instance.TestDbConnection();
            FlightCenterSystem system = FlightCenterSystem.GetInstance();
            AnonymousUserFacade anonymousUserFacade = system.GetFacade<AnonymousUserFacade>();
            Console.ReadLine();


            //   FlightDAOPGSQL flightDAOPGSQL = new FlightDAOPGSQL();
            //flightDAOPGSQL.Add(new Flight(new AirlineCompany { Id = 3 }, 4, 4, DateTime.Now.AddHours(-2), DateTime.Now,200));
            //var x =flightDAOPGSQL.GetFlightsWithTicketsAfterLanding(3 * 60 * 60);


            //ICountryDAO countryDAO = new CountryDAOPGSQL();
            //var x=countryDAO.Run_Generic_SP("sp_get_all_countries", new { });

            //var countries = countryDAO.GetAll();
            //foreach (var country in countries)
            //{
            //    Console.WriteLine(country.ToString());
            //}
            //var country = countryDAO.Get(3);
            //IAnonymousUserFacade anonymousUserFacade = new AnonymousUserFacade();
            //anonymousUserFacade.GetAllAirlineCompanies();
            //Console.WriteLine(country.ToString());

            //countryDAO.Add(new Country("UK"));
            //countryDAO.Update(new Country("Belgium",9));




            //IUserDAO userDAO = new UserDAOPGSQL();
            //userDAO.Add(new User("Airoflot", "654321", "Airoflot@gmail.com", UserRoles.Airline_Company));
            //userDAO.Add(new User("Mark2", "123456", "mark2@gmail.com", UserRoles.Customer));
            //userDAO.Add(new User("Mark3", "123456", "mark3@gmail.com", UserRoles.Customer));
            //var users = userDAO.GetAll();
            //foreach (var user in users)
            //{
            //    Console.WriteLine(user.ToString());
            //}
            //var user = userDAO.Get(2);
            //userDAO.Remove(new User { Id = 1 });
            //userDAO.Update(new User("Mark", "Aa1234", "mark@gmail.com", UserRoles.Administrator,2));

            //IAdminDAO adminDAO = new AdminDAOPGSQL();
            //adminDAO.Remove(new Administrator { Id = 1 });
            //adminDAO.Add(new Administrator("Anna", "Finkel", 1, new User { Id = 3 }));
            //var admin= adminDAO.Get(3);
            //Console.WriteLine(admin);
            //var admins = adminDAO.GetAll();
            //foreach (var admin in admins)
            //{
            //    Console.WriteLine(admin.ToString());
            //}
            //adminDAO.Update(new Administrator("Anna", "Khilchenko", 2, new User { Id = 3 }, 4));
            //IAirlineDAO airlineDAO = new AirlineDAOPGSQL();
            //Console.WriteLine(airlineDAO.GetAirlineByUsername("elal"));
            //var companies_from_israel = airlineDAO.GetAllAirlinesByCountry(3);

            //foreach (var item in companies_from_israel)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(airlineDAO.Get(2));
            //airlineDAO.Remove(new AirlineCompany { Id = 1 });
            //airlineDAO.Add(new AirlineCompany("Airoflot", 4, new User { Id = 5 }));
            //airlineDAO.Update(new AirlineCompany("Airoflot v-2", 4, new User { Id = 5 }, 3));
            //var airline_companies = airlineDAO.GetAll();
            //foreach (var airline_company in airline_companies)
            //{
            //    Console.WriteLine(airline_company.ToString());
            //}
            //ICustomerDAO customerDAO = new CustomerDAOPGSQL();
            //Console.WriteLine(customerDAO.GetCustomerByUsername("Mark2"));
            //customerDAO.Add(new Customer("Mark2", "Finkel2", "Hamasger", "0526380898", "45809781857512347", new User { Id = 6 }));
            //customerDAO.Remove(new Customer { Id = 2 });
            //customerDAO.Update(new Customer("Mark3", "Finkel3", "Hatsionut 3", "0524512786", "45806324812784164", new User { Id = 7 },3));
            //Console.WriteLine(customerDAO.Get(3).ToString());
            //var customers = customerDAO.GetAll();
            //foreach (var customer in customers)
            //{
            //    Console.WriteLine(customer.ToString());
            //}
            //IFlightDAO flightDAO = new FlightDAOPGSQL();

            //flightDAO.Add(new Flight(new AirlineCompany { Id = 2 }, 3, 7, DateTime.Now.AddHours(20), DateTime.Now.AddHours(28), 5));
            //var flights_from_israel = flightDAO.GetFlightsByOriginCountry(3);
            //var flights_to_russia = flightDAO.GetFlightsByDestinationCountry(4);
            //foreach (var item in flights_from_israel)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("---------------------------------------");
            //foreach (var item in flights_to_russia)
            //{
            //    Console.WriteLine(item);
            //}
            //var flights_by_customer = flightDAO.GetFlightsByCustomer(new Customer { Id = 1 });
            //foreach (var item in flights_by_customer)
            //{
            //    Console.WriteLine(item);
            //}
            //var flights_vacancy_dict = flightDAO.GetAllFlightsVacancy();
            //var flights_today_depature = flightDAO.GetFlightsByDepatrureDate(DateTime.Now.AddDays(2));
            //var flights_today_landing = flightDAO.GetFlightsByLandingDate(DateTime.Now.AddDays(0));
            //flightDAO.Add(new Flight(new AirlineCompany { Id = 3 }, 4, 9, DateTime.Now.AddHours(2), DateTime.Now.AddHours(4), 27));
            //Console.WriteLine(flightDAO.Get(1).ToString());
            //var flights = flightDAO.GetAll();
            //foreach (var item in flights)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            //flightDAO.Remove(new Flight { Id = 1 });
            //flightDAO.Update(new Flight(new AirlineCompany { Id = 2 }, 3, 4, DateTime.Now.AddHours(1), DateTime.Now.AddHours(5), 50, 2));
            //ITicketDAO ticketDAO = new TicketDAOPGSQL();
            //ticketDAO.Add(new Ticket(new Flight { Id = 4 }, new Customer { Id = 3 }));
            //ticketDAO.Add(new Ticket(new Flight { Id = 4 }, new Customer { Id = 1 }));
            //ticketDAO.Add(new Ticket(new Flight { Id = 2 }, new Customer { Id = 3 }));
            //Console.WriteLine(ticketDAO.Get(1));
            //Console.WriteLine("---------------------");
            //var tickets=ticketDAO.GetAll();
            //foreach (var item in tickets)
            //{
            //    Console.WriteLine(item);
            //}
            //ticketDAO.Update(new Ticket(new Flight { Id = 2 }, new Customer { Id = 1 }, 2));
            //ILoginService service = new LoginService();
            //service.TryLogin("", "", out ILoginToken token, out FacadeBase facade);
        }
    }
}
