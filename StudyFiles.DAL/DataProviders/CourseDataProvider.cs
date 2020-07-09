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
            using var command = new SqlCommand(Queries.GetCourses);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            return DBHelper.GetData(new CourseDTOMapper(), command);
        }
        public static IEntityDTO AddCourse(string teacherName, int disciplineID)
        {
            using var command = new SqlCommand(Queries.AddCourse);
            command.Parameters.Add(new SqlParameter("@teacher", SqlDbType.NVarChar) { Value = teacherName });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            var id = (int)DBHelper.ExecuteScalar(command);
            return new CourseDTO(id, teacherName, disciplineID);
        }
    }
}
