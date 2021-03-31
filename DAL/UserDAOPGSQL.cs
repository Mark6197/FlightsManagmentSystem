using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Reflection;

namespace DAL
{
    public class UserDAOPGSQL : BasicDB<User>, IUserDAO
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override long Add(User user)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_user";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("_username", user.UserName),
                    new NpgsqlParameter("_password", user.Password),
                    new NpgsqlParameter("_email", user.Email),
                    new NpgsqlParameter("_user_role_id", (int)user.UserRole),
                });
                command.CommandType = System.Data.CommandType.StoredProcedure;

                long id = (long)command.ExecuteScalar();

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

        public override User Get(long id)
        {
            User user = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_user";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = (long)reader["id"],
                        UserName = (string)reader["username"],
                        Password = (string)reader["password"],
                        Email = (string)reader["email"],
                        UserRole = (UserRoles)reader["user_role_id"]
                    };
                }

                return user;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override IList<User> GetAll()
        {
            List<User> users = new List<User>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_users";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(
                        new User
                        {
                            Id = (long)reader["id"],
                            UserName = (string)reader["username"],
                            Password = (string)reader["password"],
                            Email = (string)reader["email"],
                            UserRole = (UserRoles)reader["user_role_id"]
                        });
                }

                return users;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Remove(User user)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_user(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", user.Id));

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public override void Update(User user)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_update_user(@_id, @_username, @_password, @_email, @_user_role_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", user.Id),
                    new NpgsqlParameter("@_username", user.UserName),
                    new NpgsqlParameter("@_password", user.Password),
                    new NpgsqlParameter("@_email", user.Email),
                    new NpgsqlParameter("@_user_role_id", (int)user.UserRole)
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
