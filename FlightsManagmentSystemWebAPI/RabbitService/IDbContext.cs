using System.Collections.Generic;

namespace FlightsManagmentSystemWebAPI.RabbitService
{
    public interface IDbContext
    {
         List<Dictionary<string, string>> ReadFromDb(string sp_name, params string[] args);
         string WriteToDb(string sp_name, params string[] args);
    }
}
