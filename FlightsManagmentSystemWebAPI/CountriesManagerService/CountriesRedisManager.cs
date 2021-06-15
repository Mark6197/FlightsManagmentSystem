//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FlightsManagmentSystemWebAPI.CountriesManagerService
//{
    //public class CountriesRedisManager
    //{
    //    private static string _host = "localhost";
        //        private static readonly ICountryDAO _countryDAO = new CountryDAOPGSQL();
        //        private static readonly RedisClient _redisClient = new RedisClient(_host);

        //        static CountriesRedisManager()
        //        {
        //            ClearRedis();
        //            LoadCountries();
        //        }

        //        private static void LoadCountries()
        //        {
        //            var countries = _countryDAO.GetAll();

        //            foreach (var country in countries)
        //            {
        //                _redisClient.Set<string>(country.Id.ToString(), country.Name);
        //            }
        //        }

        //        public static void SetCountry(LoginToken<Administrator> loginToken, Country country)
        //        {
        //            _redisClient.Set<string>(country.Id.ToString(), country.Name);
        //        }

        //        public static void RemoveCountry(LoginToken<Administrator> loginToken, Country country)
        //        {
        //            _redisClient.Remove(country.Id.ToString());
        //        }

        //        public static string GetCountryName(int id)
        //        {
        //            return _redisClient.Get<string>(id.ToString());
        //        }

        //        public static void ClearRedis()
        //        {
        //            foreach (var key in _redisClient.GetAllKeys())
        //                _redisClient.Remove(key);
        //        }
    //}
//}
