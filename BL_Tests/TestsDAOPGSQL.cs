﻿using DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL_Tests
{
    internal class TestsDAOPGSQL
    {
        internal static void ClearDB()
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                string procedure = "call sp_clear_db()";

                NpgsqlCommand command = new NpgsqlCommand(procedure, conn);

                command.ExecuteNonQuery();

            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }
        }
    }
}
