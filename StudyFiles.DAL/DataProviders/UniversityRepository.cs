using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using StudyFiles.DTO;
using StudyFiles.DAL.Mappers;

namespace StudyFiles.DAL.DataProviders
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly IDBHelper _dbHelper;

        public UniversityRepository(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IEnumerable<UniversityDTO> GetUniversities()
        {
            using var command = new SqlCommand(Queries.GetUniversities);

            return _dbHelper.GetData(new UniversityDTOMapper(), command);
        }
        public IEntityDTO AddUniversity(string name)
        {
            using var command = new SqlCommand(Queries.AddUniversity);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) {Value = name});

            var id = _dbHelper.ExecuteScalar<int>(command);
            return new UniversityDTO {ID = id, InnerText = name};
        }
    }
}
