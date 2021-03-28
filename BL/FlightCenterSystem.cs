using BL.LoginService;
using ConfigurationService;
using DAL;
using Domain.Entities;
using Domain.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace BL
{
    public class FlightCenterSystem
    {

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static FlightCenterSystem _instance;
        private static readonly object key = new object();
        public readonly ILoginService loginService = new LoginService.LoginService();
        private static string _work_time = FlightsManagmentSystemConfig.Instance.WorkTime;


        private FlightCenterSystem()
        {
            Thread thread = new Thread(StoreFlightDetailsHistory);
            thread.Start(_work_time);
        }

        private static void StoreFlightDetailsHistory(object timeSpan)
        {
            if (!TimeSpan.TryParse(timeSpan.ToString(), out TimeSpan ts))
            {
                _logger.Error("Time to backup flights details history is configured in wrong format");
                return;
            }

            double secondsToGo = (ts - DateTime.Now.TimeOfDay).TotalSeconds;
            if (secondsToGo<0)
                secondsToGo += (24 * 60 * 60);

            Thread.Sleep(new TimeSpan(0, 0, (int)secondsToGo));
            _logger.Debug($"Backup will start in {secondsToGo} seconds");

            while (true)
            {
                _logger.Info("Starting backup...");

                IFlightDAO flightDAO = new FlightDAOPGSQL();
                ITicketDAO ticketDAO = new TicketDAOPGSQL();
                IFlightsTicketsHistoryDAO flightsTicketsHistoryDAO = new FlightsTicketsHistoryDAOPGSQL();
                var flights_with_tickets = flightDAO.GetFlightsWithTicketsAfterLanding(3 * 60 * 60);
                int flights_count = 0;
                int tickets_count = 0;

                foreach (Flight flight in flights_with_tickets.Keys)
                {
                    flightsTicketsHistoryDAO.Add(flight);

                    foreach (Ticket ticket in flights_with_tickets[flight])
                    {
                        if (ticket.Id != 0)
                        {
                            flightsTicketsHistoryDAO.Add(ticket);
                            ticketDAO.Remove(ticket);
                            tickets_count++;
                        }
                    }

                    flightDAO.Remove(flight);
                    flights_count++;
                }

                _logger.Info($"Backed up {flights_count} flights and {tickets_count} tickets");
                Thread.Sleep(new TimeSpan(24, 0, 0));
            }
        }

        public static FlightCenterSystem GetInstance()
        {

            if (_instance == null)
            {
                lock (key)
                {
                    if (_instance == null)
                    {
                        _instance = new FlightCenterSystem();
                    }
                }
            }

            return _instance;
        }

        public T GetFacade<T>() where T : FacadeBase, new()
        {
            return new T();
        }
    }
}
