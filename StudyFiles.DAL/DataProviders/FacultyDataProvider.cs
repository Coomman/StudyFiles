using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class FacultyDataProvider
    {
        public static List<FacultyDTO> GetFaculties(int universityID)
        {
            const string query = "Select * from Faculty Where UniversityID = @id";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = universityID});

            return DBHelper.GetData(new FacultyDTOMapper(), command);
        }
        public static IEntityDTO AddFaculty(string name, int universityID)
        {
            const string query = "Insert into Faculty ([Name], UniversityID) " +
                                 "Output Inserted.ID " +
                                 "values (@name, @id)";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = universityID });

            var id = (int)DBHelper.ExecuteScalar(command);
            return new FacultyDTO(id, name, universityID);
        }
    }
}
