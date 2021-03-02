using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class AdminDAOPGSQL : IAdminDAO
    {
        private string conn_string = "Host=localhost;Username=postgres;Password=admin;Database=flights_managment_system;";

        public void Add(Administrator admin)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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

                var id = command.ExecuteScalar();
            }
        }

        public Administrator Get(int id)
        {
            Administrator admin = new Administrator();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "sp_get_administrator";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("_id", id));
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
                            Id= (long)reader["user_id"],
                            UserName= (string)reader["username"],
                            Password= (string)reader["password"],
                            Email= (string)reader["email"],
                            UserRole=(UserRoles)reader["user_role_id"]
                        }
                    };
                }
            }
            return admin;
        }

        public Administrator GetAdministratorByUsernameAndPassword(string username, string password)
        {
            Administrator admin = new Administrator();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return admin;
        }
    

        public IList<Administrator> GetAll()
        {
            List<Administrator> administrators = new List<Administrator>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return administrators;
        }

        public void Remove(Administrator admin)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_administrator(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", admin.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public void Update(Administrator admin)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
