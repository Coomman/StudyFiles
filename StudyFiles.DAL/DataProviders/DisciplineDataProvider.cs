using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class DisciplineDataProvider
    {
        public static List<DisciplineDTO> GetDisciplines(int facultyID)
        {
            using var command = new SqlCommand(Queries.GetDisciplines);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            return DBHelper.GetData(new DisciplineDTOMapper(), command);
        }
        public static IEntityDTO AddDiscipline(string name, int facultyID)
        {
            using var command = new SqlCommand(Queries.AddDiscipline);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            var id = (int)DBHelper.ExecuteScalar(command);
            return new DisciplineDTO {ID = id, FacultyID = facultyID, InnerText = name};
        }
    }
}
