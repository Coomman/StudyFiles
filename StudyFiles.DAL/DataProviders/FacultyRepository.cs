using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly IDBHelper _dbHelper;

        public FacultyRepository(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IEnumerable<FacultyDTO> GetFaculties(int universityID)
        {
            using var command = new SqlCommand(Queries.GetFaculties);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = universityID});

            return _dbHelper.GetData(new FacultyDTOMapper(), command);
        }
        public IEntityDTO AddFaculty(string name, int universityID)
        {
            using var command = new SqlCommand(Queries.AddFaculty);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = universityID });

            var id = _dbHelper.ExecuteScalar<int>(command);
            return new FacultyDTO {ID = id, InnerText = name, UniversityID = universityID};
        }
    }
}
