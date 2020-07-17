using System.Collections.Generic;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;

namespace StudyFiles.DAL
{
    public interface IDBHelper
    {
        public IReadOnlyList<T> GetData<T>(IMapper<T> mapper, SqlCommand command);

        public T GetItem<T>(IMapper<T> mapper, SqlCommand command);

        public void ExecuteNonQuery(SqlCommand command);

        public T ExecuteScalar<T>(SqlCommand command);
    }
}
