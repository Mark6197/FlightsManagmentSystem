using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class UserDAOPGSQL : BasicDB<User>, IUserDAO
    {
        public override void Add(User user)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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

                var id = command.ExecuteScalar();
            }
        }

        public override User Get(int id)
        {
            User user = new User();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return user;
        }

        public override IList<User> GetAll()
        {
            List<User> users = new List<User>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
            }
            return users;
        }

        public override void Remove(User user)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string procedure = "call sp_remove_user(@_id)";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);
                command.Parameters.Add(new NpgsqlParameter("@_id", user.Id));
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }

        public override void Update(User user)
        {
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
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
                //command.CommandType = System.Data.CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
        }
    }
}
