using BL.Interfaces;
using DAL;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BL
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnonymousUserFacade() : base()
        {
        }

        public AirlineCompany GetAirlineCompanyById(long id)
        {
            AirlineCompany result = null;

            result = Execute(() => _airlineDAO.Get(id), new { Id = id }, _logger);

            return result;

        }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            IList<AirlineCompany> result = null;

            result = Execute(() => _airlineDAO.GetAll(), new { }, _logger);

            return result;

        }

        public IList<Country> GetAllCountries()
        {
            IList<Country> result = null;

            result = Execute(() => _countryDAO.GetAll(), new { }, _logger);

            return result;
        }

        public IList<Flight> GetAllFlights()
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetAll(), new { }, _logger);

            return result;
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> result = null;

            result = Execute(() => _flightDAO.GetAllFlightsVacancy(), new { }, _logger);

            return result;
        }

        public Country GetCountryById(int id)
        {
            Country result = null;

            result = Execute(() => _countryDAO.Get(id), new { Id = id }, _logger);

            return result;
        }

        public Flight GetFlightById(long id)
        {
            Flight result = null;

            result = Execute(() => _flightDAO.Get(id), new { Id = id }, _logger);

            return result;
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByDepatrureDate(departureDate), new { DepartureDate = departureDate }, _logger);

            return result;
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryId)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByDestinationCountry(countryId), new { CountryId = countryId }, _logger);

            return result;
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByLandingDate(landingDate), new { LandingDate = landingDate }, _logger);

            return result;
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryId)
        {
            IList<Flight> result = null;

            result = Execute(() => _flightDAO.GetFlightsByOriginCountry(countryId), new { CountryId = countryId }, _logger);

            return result;
        }
    }
}
