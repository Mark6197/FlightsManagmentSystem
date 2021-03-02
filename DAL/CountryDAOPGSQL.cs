using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class CountryDAOPGSQL : ICountryDAO
    {
        private string conn_string = "Host=localhost;Username=postgres;Password=admin;Database=flights_managment_system;";

        public void Add(Country country)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_add_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_name", country.Name));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var id = command.ExecuteScalar();
            }
        }


        public Country Get(int id)
        {
            Country country = new Country();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return country;
        }

        public IList<Country> GetAll()
        {
            List<Country> countries = new List<Country>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return countries;
        }

        public void Remove(Country country)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_country(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", country.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public void Update(Country country)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_update_country(@_id, @_name)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", country.Id),
                    new NpgsqlParameter("@_name", country.Name)
                });
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
