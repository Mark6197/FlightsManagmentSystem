using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DAL
{
    public class AdminDAOPGSQL : BasicDB<Administrator>, IAdminDAO
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override long Add(Administrator admin)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_add_administrator";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                        new NpgsqlParameter("_first_name", admin.FirstName),
                        new NpgsqlParameter("_last_name", admin.LastName),
                        new NpgsqlParameter("_level", admin.Level),
                        new NpgsqlParameter("_user_id", admin.User.Id),
                });
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

        public override Administrator Get(long id)
        {
            Administrator admin = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();
                string procedure = "sp_get_administrator";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", (int)id));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    admin = new Administrator
                    {
                        Id = (int)reader["admin_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Level = (int)reader["level"],
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

                return admin;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }

        public Administrator GetAdministratorByUsernameAndPassword(string username, string password)
        {
            Administrator admin = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_administrator_by_username_and_password";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_username", username));
                command.Parameters.Add(new NpgsqlParameter("_password", password));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    admin = new Administrator
                    {
                        Id = (int)reader["admin_id"],
                        FirstName = (string)reader["first_name"],
                        LastName = (string)reader["last_name"],
                        Level = (int)reader["level"],
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
                else
                {
                    return null;
                }

                return admin;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }

        }


        public override IList<Administrator> GetAll()
        {
            List<Administrator> administrators = new List<Administrator>();
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "sp_get_all_administrators";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    administrators.Add(
                        new Administrator
                        {
                            Id = (int)reader["admin_id"],
                            FirstName = (string)reader["first_name"],
                            LastName = (string)reader["last_name"],
                            Level = (int)reader["level"],
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

                return administrators;
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }

        }

        public override void Remove(Administrator admin)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_remove_administrator(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", admin.Id));

                command.ExecuteNonQuery();
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }

        }

        public override void Update(Administrator admin)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_update_administrator(@_id, @_first_name, @_last_name, @_level, @_user_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@_id", admin.Id),
                    new NpgsqlParameter("@_first_name", admin.FirstName),
                    new NpgsqlParameter("@_last_name", admin.LastName),
                    new NpgsqlParameter("@_level", admin.Level),
                    new NpgsqlParameter("@_user_id", admin.User.Id)
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
