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
            const string query = "Select * from Discipline Where FacultyID = @id";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            return DBHelper.GetData(new DisciplineDTOMapper(), command);
        }
        public static IEntityDTO AddDiscipline(string name, int facultyID)
        {
            const string query = "Insert into Discipline ([Name], FacultyID) " +
                                 "Output Inserted.ID " +
                                 "values (@name, @id)";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = facultyID });

            var id = (int)DBHelper.ExecuteScalar(command);
            return new DisciplineDTO(id, name, facultyID);
        }
    }
}
