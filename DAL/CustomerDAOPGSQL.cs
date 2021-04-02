using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Reflection;

namespace DAL
{
    public class CustomerDAOPGSQL : BasicDB<Customer>, ICustomerDAO
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override long Add(Customer customer)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            long result = 0;

            result = Execute(() =>
            {
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

                result = (long)command.ExecuteScalar();

                return result;
            }, new { Customer = customer }, conn, _logger);

            return result;
        }

        public override Customer Get(long id)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            Customer result = null;
            result = Execute(() =>
            {
                string procedure = "sp_get_customer";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = new Customer
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

                return result;
            }, new { Id = id }, conn, _logger);

            return result;
        }

        public override IList<Customer> GetAll()
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            List<Customer> result = new List<Customer>();

            result = Execute(() =>
            {
                string procedure = "sp_get_all_customers";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(
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

                return result;
            }, new { }, conn, _logger);

            return result;
        }

        public Customer GetCustomerByUsername(string username)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();
            Customer result = null;
            result = Execute(() =>
            {
                string procedure = "sp_get_customer_by_username";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = new Customer
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

                return result;
            }, new { Username = username }, conn, _logger);

            return result;
        }

        public override void Remove(Customer customer)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
                string procedure = "call sp_remove_customer(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", customer.Id));

                command.ExecuteNonQuery();
            }, new { Customer = customer }, conn, _logger);
        }

        public override void Update(Customer customer)
        {
            NpgsqlConnection conn = DbConnectionPool.Instance.GetConnection();

            Execute(() =>
            {
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
            }, new { Customer = customer }, conn, _logger);
        }
    }
}
