﻿using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class AirlineDAOPGSQL : IAirlineDAO
    {
        private string conn_string = "Host=localhost;Username=postgres;Password=admin;Database=flights_managment_system;";

        public void Add(AirlineCompany airlineCompany)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_add_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_name", airlineCompany.Name),
                    new NpgsqlParameter("_country_id", airlineCompany.CountryId),
                    new NpgsqlParameter("_user_id", airlineCompany.User.Id)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var id = command.ExecuteScalar();
            }
        }

        public AirlineCompany Get(int id)
        {
            AirlineCompany airlineCompany = new AirlineCompany();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_airline_company";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new AirlineCompany
                    {
                        Id = (long)reader["airline_company_id"],
                        Name = (string)reader["name"],
                        CountryId = (int)reader["country_id"],
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
            }
            return airlineCompany;
        }

        public AirlineCompany GetAirlineByUsername(string username)
        {
            AirlineCompany airlineCompany = new AirlineCompany();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_airline_company_by_username";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new AirlineCompany
                    {
                        Id = (long)reader["airline_company_id"],
                        Name = (string)reader["name"],
                        CountryId = (int)reader["country_id"],
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
            }
            return airlineCompany;
        }

        public AirlineCompany GetAirlineByUsernameAndPassword(string username, string password)
        {
            AirlineCompany airlineCompany = new AirlineCompany();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_airline_company_by_username_and_password";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.Parameters.Add(new NpgsqlParameter("_password", password));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new AirlineCompany
                    {
                        Id = (long)reader["airline_company_id"],
                        Name = (string)reader["name"],
                        CountryId = (int)reader["country_id"],
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
            }
            return airlineCompany;
        }

        public IList<AirlineCompany> GetAll()
        {
            List<AirlineCompany> airline_companies = new List<AirlineCompany>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_all_airline_companies";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    airline_companies.Add(
                        new AirlineCompany
                        {
                            Id = (long)reader["airline_company_id"],
                            Name = (string)reader["name"],
                            CountryId = (int)reader["country_id"],
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
            }
            return airline_companies;
        }

        public IList<AirlineCompany> GetAllAirlinesByCountry(int countryId)
        {
            List<AirlineCompany> airline_companies = new List<AirlineCompany>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_all_airline_companies_by_country";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_country_id", countryId));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    airline_companies.Add(
                        new AirlineCompany
                        {
                            Id = (long)reader["airline_company_id"],
                            Name = (string)reader["name"],
                            CountryId = (int)reader["country_id"],
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
            }
            return airline_companies;
        }

        public void Remove(AirlineCompany airlineCompany)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_airline_company(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", airlineCompany.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public void Update(AirlineCompany airlineCompany)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_update_airline_company(@_id, @_name, @_country_id, @_user_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", airlineCompany.Id),
                    new NpgsqlParameter("@_name", airlineCompany.Name),
                    new NpgsqlParameter("@_country_id", airlineCompany.CountryId),
                    new NpgsqlParameter("@_user_id", airlineCompany.User.Id)
                });
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
