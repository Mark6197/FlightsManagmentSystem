using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IAirlineDAO : IBasicDB<AirlineCompany>
    {
        AirlineCompany GetAirlineByUsername(string username);
        IList<AirlineCompany> GetAllAirlinesByCountry(int countryId);
    }
}
