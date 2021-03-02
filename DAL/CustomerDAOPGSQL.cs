using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class CustomerDAOPGSQL : ICustomerDAO
    {
        private string conn_string = "Host=localhost;Username=postgres;Password=admin;Database=flights_managment_system;";

        public void Add(Customer customer)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_add_customer";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_first_name", customer.FirstName),
                    new NpgsqlParameter("_last_name", customer.LastName),
                    new NpgsqlParameter("_address", customer.Address),
                    new NpgsqlParameter("_phone_number", customer.PhoneNumber),
                    new NpgsqlParameter("_credit_card_number", customer.CreditCardNumber),
                    new NpgsqlParameter("_user_id", customer.User.Id)
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var id = command.ExecuteScalar();
            }
        }

        public Customer Get(int id)
        {
            Customer airlineCompany = new Customer();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_customer";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new Customer
                    {
                        Id = (long)reader["customer_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Address = (string)reader["address"],
                        PhoneNumber = (string)reader["phone_number"],
                        CreditCardNumber = (string)reader["credit_card_number"],
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

        public IList<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_all_customers";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(
                        new Customer
                        {
                            Id = (long)reader["customer_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            Address = (string)reader["address"],
                            PhoneNumber = (string)reader["phone_number"],
                            CreditCardNumber = (string)reader["credit_card_number"],
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
            return customers;
        }

        public Customer GetCustomerByUsername(string username)
        {
            Customer airlineCompany = new Customer();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_customer_by_username";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new Customer
                    {
                        Id = (long)reader["customer_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Address = (string)reader["address"],
                        PhoneNumber = (string)reader["phone_number"],
                        CreditCardNumber = (string)reader["credit_card_number"],
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

        public Customer GetCustomerByUsernameAndPassword(string username, string password)
        {
            Customer airlineCompany = new Customer();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_customer_by_username_and_password";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.Parameters.Add(new NpgsqlParameter("_password", password));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    airlineCompany = new Customer
                    {
                        Id = (long)reader["customer_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Address = (string)reader["address"],
                        PhoneNumber = (string)reader["phone_number"],
                        CreditCardNumber = (string)reader["credit_card_number"],
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

        public void Remove(Customer customer)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_customer(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", customer.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public void Update(Customer customer)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_update_customer(@_id, @_first_name, @_last_name, @_address, @_phone_number, @_credit_card_number, @_user_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", customer.Id),
                    new NpgsqlParameter("@_first_name", customer.FirstName),
                    new NpgsqlParameter("@_last_name", customer.LastName),
                    new NpgsqlParameter("@_address", customer.Address),
                    new NpgsqlParameter("@_phone_number", customer.PhoneNumber),
                    new NpgsqlParameter("@_credit_card_number", customer.CreditCardNumber),
                    new NpgsqlParameter("@_user_id", customer.User.Id)
                });
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
