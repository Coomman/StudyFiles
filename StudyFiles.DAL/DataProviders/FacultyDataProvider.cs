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
            using var command = new SqlCommand(Queries.GetFaculties);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = universityID});

            return DBHelper.GetData(new FacultyDTOMapper(), command);
        }
        public static IEntityDTO AddFaculty(string name, int universityID)
        {
            using var command = new SqlCommand(Queries.AddFaculty);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = name });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = universityID });

            var id = (int) DBHelper.ExecuteScalar(command);
            return new FacultyDTO {ID = id, InnerText = name, UniversityID = universityID};
        }
    }
}
