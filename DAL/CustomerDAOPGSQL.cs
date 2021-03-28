using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System.Collections.Generic;

namespace DAL
{
    public class CustomerDAOPGSQL : BasicDB<Customer>, ICustomerDAO
    {
        public override long Add(Customer customer)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

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

                long id = (long)command.ExecuteScalar();

                return id;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override Customer Get(int id)
        {
            Customer customer = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_customer";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer
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

                return customer;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override IList<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

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

                return customers;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public Customer GetCustomerByUsername(string username)
        {
            Customer customer = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_customer_by_username";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer
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

                return customer;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public Customer GetCustomerByUsernameAndPassword(string username, string password)
        {
            Customer customer = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_customer_by_username_and_password";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.Parameters.Add(new NpgsqlParameter("_password", password));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer
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

                return customer;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Remove(Customer customer)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_customer(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", customer.Id));

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Update(Customer customer)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

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

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }
    }
}
