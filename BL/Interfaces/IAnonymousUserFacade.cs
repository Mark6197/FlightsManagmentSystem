using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IAnonymousUserFacade
    {
        IList<Flight> GetAllFlights();
        IList<AirlineCompany> GetAllAirlineCompanies();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        Flight GetFlightById(int id);
        IList<Flight> GetFlightsByOriginCountry(int countryId);
        IList<Flight> GetFlightsByDestinationCountry(int countryId);
        IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
    }
}
