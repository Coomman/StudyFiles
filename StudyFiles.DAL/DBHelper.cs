using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.Logging;

using ILogger = StudyFiles.Logging.ILogger;

namespace StudyFiles.DAL
{
    public sealed class DBHelper : IDBHelper
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<DBHelper>();

        private readonly SqlConnection _connection;

        private void OpenConnection()
        {
            _connection.Open();
        }

        public DBHelper()
        {
            try
            {
                _connection = new SqlConnection(Queries.DefaultConnection);
                OpenConnection();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error occured on connection open");
            }
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
                Logger.Error(ex, $"Query: {command.CommandText}");
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
                Logger.Error(ex, $"Query: {command.CommandText}");
            }

            return result;
        }

        public void ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                command.Connection = _connection;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Query: {command.CommandText}");
            }
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
                Logger.Error(ex, $"Query: {command.CommandText}");
            }

            return default;
        }
    }
}
