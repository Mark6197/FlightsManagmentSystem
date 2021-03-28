using BL.Interfaces;
using DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BL
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public AnonymousUserFacade()
        {
            _airlineDAO = new AirlineDAOPGSQL();
            _countryDAO = new CountryDAOPGSQL();
            _flightDAO = new FlightDAOPGSQL();
        }

        public AirlineCompany GetAirlineCompanyById(long id)
        {

            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({id})");

            var result = _airlineDAO.Get((int)id);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;

        }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _airlineDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Country> GetAllCountries()
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _countryDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Flight> GetAllFlights()
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _flightDAO.GetAll();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _flightDAO.GetAllFlightsVacancy();

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public Country GetCountryById(int id)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _countryDAO.Get(id);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public Flight GetFlightById(int id)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}()");

            var result = _flightDAO.Get(id);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {

            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({departureDate})");

            var result = _flightDAO.GetFlightsByDepatrureDate(departureDate);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryId)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({countryId})");

            var result = _flightDAO.GetFlightsByDestinationCountry(countryId);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({landingDate})");

            var result = _flightDAO.GetFlightsByLandingDate(landingDate);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryId)
        {
            _logger.Debug($"Entering {MethodBase.GetCurrentMethod().Name}({countryId})");

            var result = _flightDAO.GetFlightsByOriginCountry(countryId);

            _logger.Debug($"Leaving {MethodBase.GetCurrentMethod().Name}. Result: {result}");

            return result;
        }
    }
}
