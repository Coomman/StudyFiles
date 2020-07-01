using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class CourseDataProvider
    {
        public static List<CourseDTO> GetCourses(int disciplineID)
        {
            const string query = "Select * from Course Where DisciplineID = @id";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            return DBHelper.GetData(new CourseDTOMapper(), command);
        }
        public static IEntityDTO AddCourse(string teacherName, int disciplineID)
        {
            const string query = "Insert into Course ([Teacher], DisciplineID) " +
                                 "Output Inserted.ID " +
                                 "values (@teacher, @id)";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@teacher", SqlDbType.NVarChar) { Value = teacherName });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            var id = (int)DBHelper.ExecuteScalar(command);
            return new CourseDTO(id, teacherName, disciplineID);
        }
    }
}
