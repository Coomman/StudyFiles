using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using System.Configuration;

namespace StudyFiles.DAL
{
    public static class DBAccessor
    {
        public static List<T> GetData<T>(IMapper<T> mapper, string queryString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using var connection = new SqlConnection(connectionString);

            return GetData(connection, mapper, queryString);
        }

        private static List<T> GetData<T>(SqlConnection connection, IMapper<T> mapper, string queryString)
        {
            var result = new List<T>();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var command = new SqlCommand(queryString, connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                    result.Add(mapper.ReadItem(reader));

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public static T GetItem<T>(IMapper<T> mapper, string queryString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using var connection = new SqlConnection(connectionString);

            return GetItem(connection, mapper, queryString);
        }

        private static T GetItem<T>(SqlConnection connection, IMapper<T> mapper, string queryString)
        {
            T result = default(T);

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var command = new SqlCommand(queryString, connection);

                var reader = command.ExecuteReader();
                reader.Read();
                result = mapper.ReadItem(reader);

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
