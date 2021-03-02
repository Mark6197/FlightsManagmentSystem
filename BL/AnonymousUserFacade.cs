using BL.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }

        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightsVacancy();
        }

        public Flight GetFlightById(int id)
        {
            return _flightDAO.Get(id);
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightsByDepatrureDate(departureDate);
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryId)
        {
            return _flightDAO.GetFlightsByDestinationCountry(countryId);
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightsByLandingDate(landingDate);
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryId)
        {
            return _flightDAO.GetFlightsByOriginCountry(countryId);
        }
    }
}
