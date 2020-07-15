using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;

namespace StudyFiles.DAL
{
    public static class DBHelper
    {
        public static List<T> GetData<T>(IMapper<T> mapper, SqlCommand command)
        {
            var connectionString = Queries.DefaultConnection;

            using var connection = new SqlConnection(connectionString);

            return GetData(connection, mapper, command);
        }

        private static List<T> GetData<T>(SqlConnection connection, IMapper<T> mapper, SqlCommand command)
        {
            var result = new List<T>();

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.Connection = connection;

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

        public static T GetItem<T>(IMapper<T> mapper, SqlCommand command)
        {
            var connectionString = Queries.DefaultConnection;

            using var connection = new SqlConnection(connectionString);

            return GetItem(connection, mapper, command);
        }

        private static T GetItem<T>(SqlConnection connection, IMapper<T> mapper, SqlCommand command)
        {
            T result = default;

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.Connection = connection;

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

        public static object ExecuteScalar(SqlCommand command)
        {
            var connectionString = Queries.DefaultConnection;

            using var connection = new SqlConnection(connectionString);

            return ExecuteScalar(connection, command);
        }
        private static object ExecuteScalar(SqlConnection connection, SqlCommand command)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.Connection = connection;

                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
