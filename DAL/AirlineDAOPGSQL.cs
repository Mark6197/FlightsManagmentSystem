using Domain.Entities;
using Domain.Interfaces;
using log4net;
using Npgsql;
using System.Collections.Generic;
using System.Reflection;

namespace DAL
{
    public class AirlineDAOPGSQL : BasicDB<AirlineCompany>, IAirlineDAO
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override long Add(AirlineCompany airlineCompany)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            long result = 0;

            result = Execute(() =>
            {
                string procedure = "sp_add_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_name", airlineCompany.Name),
                    new NpgsqlParameter("_country_id", airlineCompany.CountryId),
                    new NpgsqlParameter("_user_id", airlineCompany.User.Id)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = (long)command.ExecuteScalar();

                return result;
            }, new { AirlineCompany = airlineCompany }, conn, _logger);

            return result;
        }

        public override AirlineCompany Get(long id)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            AirlineCompany result = null;

            result = Execute(() =>
            {
                string procedure = "sp_get_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = new AirlineCompany
                    {
                        Id = (long)reader["airline_company_id"],
                        Name = (string)reader["airline_company_name"],
                        CountryId = (int)reader["airline_company_country_id"],
                        User = new User
                        {
                            Id = (long)reader["user_id"],
                            UserName = (string)reader["username"],
                            Password = (string)reader["password"],
                            Email = (string)reader["email"],
                            UserRole = (UserRoles)reader["user_role_id"]
                        }
                    };
                }

                return result;
            }, new { Id = id }, conn, _logger);

            return result;
        }

        public AirlineCompany GetAirlineByUsername(string username)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            AirlineCompany result = null;

            result = Execute(() =>
            {
                string procedure = "sp_get_airline_company_by_username";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = new AirlineCompany
                    {
                        Id = (long)reader["airline_company_id"],
                        Name = (string)reader["airline_company_name"],
                        CountryId = (int)reader["airline_company_country_id"],
                        User = new User
                        {
                            Id = (long)reader["user_id"],
                            UserName = (string)reader["username"],
                            Password = (string)reader["password"],
                            Email = (string)reader["email"],
                            UserRole = (UserRoles)reader["user_role_id"]
                        }
                    };
                }

                return result;
            }, new { Username = username }, conn, _logger);

            return result;

        }

        public override IList<AirlineCompany> GetAll()
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            List<AirlineCompany> result = new List<AirlineCompany>();

            result = Execute(() =>
            {
                string procedure = "sp_get_all_airline_companies";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(
                        new AirlineCompany
                        {
                            Id = (long)reader["airline_company_id"],
                            Name = (string)reader["airline_company_name"],
                            CountryId = (int)reader["airline_company_country_id"],
                            User = new User
                            {
                                Id = (long)reader["user_id"],
                                UserName = (string)reader["username"],
                                Password = (string)reader["password"],
                                Email = (string)reader["email"],
                                UserRole = (UserRoles)reader["user_role_id"]
                            }
                        });
                }

                return result;
            }, new { }, conn, _logger);

            return result;
        }

        public IList<AirlineCompany> GetAllAirlinesByCountry(int countryId)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            List<AirlineCompany> result = new List<AirlineCompany>();

            result = Execute(() =>
            {

                string procedure = "sp_get_all_airline_companies_by_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_country_id", countryId));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(
                        new AirlineCompany
                        {
                            Id = (long)reader["airline_company_id"],
                            Name = (string)reader["airline_company_name"],
                            CountryId = (int)reader["airline_company_country_id"],
                            User = new User
                            {
                                Id = (long)reader["user_id"],
                                UserName = (string)reader["username"],
                                Password = (string)reader["password"],
                                Email = (string)reader["email"],
                                UserRole = (UserRoles)reader["user_role_id"]
                            }
                        });
                }

                return result;
            }, new { CountryId = countryId }, conn, _logger);

            return result;
        }

        public override void Remove(AirlineCompany airlineCompany)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
                string procedure = "call sp_remove_airline_company(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", airlineCompany.Id));

                command.ExecuteNonQuery();
            }, new { AirlineCompany = airlineCompany }, conn, _logger);
        }

        public override void Update(AirlineCompany airlineCompany)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
                string procedure = "call sp_update_airline_company(@_id, @_name, @_country_id, @_user_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", airlineCompany.Id),
                    new NpgsqlParameter("@_name", airlineCompany.Name),
                    new NpgsqlParameter("@_country_id", airlineCompany.CountryId),
                    new NpgsqlParameter("@_user_id", airlineCompany.User.Id)
                });

                command.ExecuteNonQuery();
            }, new { AirlineCompany = airlineCompany }, conn, _logger);
        }
    }
}
