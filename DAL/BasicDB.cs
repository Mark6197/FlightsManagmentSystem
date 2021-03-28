﻿using ConfigurationService;
using Domain.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public abstract class BasicDB<T> : IBasicDB<T> where T : IPoco, new()
    {
        protected string conn_string = FlightsManagmentSystemConfig.Instance.ConnectionString;

        public abstract long Add(T t);

        public abstract T Get(int id);//maybe should be long?

        public abstract IList<T> GetAll();

        public abstract void Remove(T t);
        private NpgsqlParameter[] GetParametersFromDataHolder(object dataObject)
        {
            List<NpgsqlParameter> paraResult = new List<NpgsqlParameter>();
            var props_in_dataObject = dataObject.GetType().GetProperties();
            foreach (var prop in props_in_dataObject)
            {
                paraResult.Add(new NpgsqlParameter(prop.Name, prop.GetValue(dataObject)));
            }
            return paraResult.ToArray();
        }

        public List<T> Run_Generic_SP(string sp_name, object dataHolder)
        {
            List<T> result = new List<T>();
            NpgsqlParameter[] param = null;
            NpgsqlConnection conn = null;

            try
            {
                conn = DbConnectionPool.Instance.GetConnection();

                NpgsqlCommand command = new NpgsqlCommand(sp_name, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure; // this is default

                param = GetParametersFromDataHolder(dataHolder);

                command.Parameters.AddRange(param);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T one_row = new T();
                    Type type_of_t = typeof(T);
                    foreach (var prop in type_of_t.GetProperties())
                    {
                        string column_name = prop.Name;

                        var custom_attr_column_name =
                            (ColumnAttribute[])prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                        if (custom_attr_column_name.Length > 0)
                            column_name = custom_attr_column_name[0].Name;

                        var value = reader[column_name];

                        prop.SetValue(one_row, value);
                    }

                    result.Add(one_row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                string params_string = "";
                foreach (var item in param)
                {
                    if (params_string != "")
                        params_string += ", ";
                    params_string += $"Name : {item.ParameterName} value: {item.Value}";
                }
                Console.WriteLine($"Function {sp_name} failed. parameters: {params_string}");
            }
            finally
            {
                DbConnectionPool.Instance.ReturnConnection(conn);
            }

            return result;
        }

        public abstract void Update(T t);
    }
}
