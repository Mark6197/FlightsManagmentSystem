using BL.LoginService;
using DAL;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Concurrent;

namespace FlightsManagmentSystemWebAPI.CountriesManagerService
{
    public class CountriesManager : ICountriesManager
    {

        private static ConcurrentDictionary<int, string> _countriesDictionary;
        private static readonly ICountryDAO _countryDAO = new CountryDAOPGSQL();

        public CountriesManager()
        {
            _countriesDictionary = new ConcurrentDictionary<int, string>();
            LoadCountries();
        }

        public void LoadCountries()
        {
            var countries = _countryDAO.GetAll();
            foreach (var country in countries)
                _countriesDictionary.TryAdd(country.Id, country.Name);
        }

        public void SetCountry(LoginToken<Administrator> loginToken, Country country)
        {
            if (_countriesDictionary.TryAdd(country.Id, country.Name))
                _countriesDictionary[country.Id] = country.Name;
        }

        public void RemoveCountry(LoginToken<Administrator> loginToken, Country country)
        {
            _countriesDictionary.TryRemove(country.Id, out string name);
        }


        public string GetCountryName(int id)
        {
            string name;

            if (!_countriesDictionary.TryGetValue(id, out name))
                RefreshCountries(null);

            if (!_countriesDictionary.TryGetValue(id, out name))
                throw new CountryNotFoundException($"Country id: {id} not exists");

            return name;
        }

        public void RefreshCountries(LoginToken<Administrator> loginToken)
        {
            _countriesDictionary.Clear();
            LoadCountries();
        }
    }
}
