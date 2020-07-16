using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using StudyFiles.DAL.Mappers;

namespace StudyFiles.DAL
{
    public sealed class DBHelper : IDBHelper
    {
        private readonly SqlConnection _connection;

        public DBHelper()
        {
            _connection = new SqlConnection(Queries.DefaultConnection);
            _connection.Open();
        }

        public IReadOnlyList<T> GetData<T>(IMapper<T> mapper, SqlCommand command)
        {
            var result = new List<T>();

            try
            {
                command.Connection = _connection;

                var reader = command.ExecuteReader();
                while (reader.Read())
                    result.Add(mapper.ReadItem(reader));

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); //TODO: Add logs
            }

            return result;
        }

        public T GetItem<T>(IMapper<T> mapper, SqlCommand command)
        {
            T result = default;

            try
            {
                command.Connection = _connection;

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

        public T ExecuteScalar<T>(SqlCommand command)
        {
            try
            {
                command.Connection = _connection;

                return (T) command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }
    }
}
