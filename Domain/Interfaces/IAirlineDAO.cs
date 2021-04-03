using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IAirlineDAO : IBasicDB<AirlineCompany>
    {
        AirlineCompany GetAirlineCompanyByUserId(long user_id);
        AirlineCompany GetAirlineByUsername(string username);
        IList<AirlineCompany> GetAllAirlinesByCountry(int countryId);
    }
}
