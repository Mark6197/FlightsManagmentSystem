﻿using Domain.Entities;
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
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            long result = 0;

            result = Execute(() =>
            {
                string procedure = "sp_add_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_name", country.Name));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                result = (int)command.ExecuteScalar();

                return result;
            }, new { Country = country }, conn, _logger);

            return result;
        }

        public override Country Get(long id)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            Country result = null;

            result = Execute(() =>
            {
                string procedure = "sp_get_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", (int)id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = new Country
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["name"]
                    };
                }

                return result;
            }, new { Id = id }, conn, _logger);

            return result;
        }

        public override IList<Country> GetAll()
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            List<Country> result = new List<Country>();

            result = Execute(() =>
            {
                string procedure = "sp_get_all_countries";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(
                        new Country
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        });
                }

                return result;
            }, new { }, conn, _logger);

            return result;
        }

        public override void Remove(Country country)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
                string procedure = "call sp_remove_country(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", country.Id));

                command.ExecuteNonQuery();
            }, new { Country = country }, conn, _logger);
        }

        public override void Update(Country country)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
                string procedure = "call sp_update_country(@_id, @_name)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", country.Id),
                    new NpgsqlParameter("@_name", country.Name)
                });

                command.ExecuteNonQuery();
            }, new { Country = country }, conn, _logger);
        }
    }
}
