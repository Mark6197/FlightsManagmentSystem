using System;
using System.Collections.Generic;

namespace FlightsManagmentSystemWebAPI.RabbitService
{
    /// <summary>
    /// Static class that 
    /// </summary>
    public static class DbContext
    {
        public static List<Dictionary<string, string>> ReadFromDb(string sp_name, params string[] args)
        {
            return new List<Dictionary<string, string>>();
        }
        public static string WriteToDb(string sp_name, params string[] args)
        {
            return Guid.NewGuid().ToString();
        }
    }
}