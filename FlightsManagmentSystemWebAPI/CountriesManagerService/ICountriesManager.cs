using BL.LoginService;
using Domain.Entities;

namespace FlightsManagmentSystemWebAPI.CountriesManagerService
{
    public interface ICountriesManager
    {
        string GetCountryName(int id);
        void RefreshCountries(LoginToken<Administrator> loginToken);
        void RemoveCountry(LoginToken<Administrator> loginToken, Country country);
        void SetCountry(LoginToken<Administrator> loginToken, Country country);
    }
}