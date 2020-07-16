using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly IDBHelper _dbHelper;

        public DisciplineRepository(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IEnumerable<DisciplineDTO> GetDisciplines(int facultyID)
        {
            using var command = new SqlCommand(Queries.GetDisciplines);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            return _dbHelper.GetData(new DisciplineDTOMapper(), command);
        }
        public IEntityDTO AddDiscipline(string name, int facultyID)
        {
            using var command = new SqlCommand(Queries.AddDiscipline);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            var id = _dbHelper.ExecuteScalar<int>(command);
            return new DisciplineDTO {ID = id, FacultyID = facultyID, InnerText = name};
        }
    }
}
