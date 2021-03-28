using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Reflection;

namespace DAL
{
    public class CountryDAOPGSQL : BasicDB<Country>, ICountryDAO
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override long Add(Country country)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_name", country.Name));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                int id = (int)command.ExecuteScalar();

                return id;

            }
            //catch (NpgsqlException ex)
            //{
            //    _logger.Error($"Message: {ex.Message}\nStack Trace:{ex.StackTrace}");
            //    return 0;
            //}
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override Country Get(int id)
        {
            Country country = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    country = new Country
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["name"]
                    };
                }

                return country;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override IList<Country> GetAll()
        {
            List<Country> countries = new List<Country>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_countries";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    countries.Add(
                        new Country
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        });
                }

                return countries;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Remove(Country country)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_country(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", country.Id));

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Update(Country country)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_update_country(@_id, @_name)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", country.Id),
                    new NpgsqlParameter("@_name", country.Name)
                });

                command.ExecuteNonQuery();
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }

        }
    }
}
