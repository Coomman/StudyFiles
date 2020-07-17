using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers.Catalog;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Catalog
{
    public sealed class CourseRepository : ICourseRepository
    {
        private readonly IDBHelper _dbHelper;

        public CourseRepository(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IEnumerable<CourseDTO> GetCourses(int disciplineID)
        {
            using var command = new SqlCommand(Queries.GetCourses);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            return _dbHelper.GetData(new CourseDTOMapper(), command);
        }
        public IEntityDTO AddCourse(string teacherName, int disciplineID)
        {
            using var command = new SqlCommand(Queries.AddCourse);
            command.Parameters.Add(new SqlParameter("@teacher", SqlDbType.NVarChar) { Value = teacherName });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = disciplineID });

            var id = _dbHelper.ExecuteScalar<int>(command);
            return new CourseDTO{ID = id, DisciplineID = disciplineID, InnerText = teacherName};
        }

        public void DeleteCourse(int id)
        {
            using var command = new SqlCommand(Queries.DeleteCourse);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            _dbHelper.ExecuteNonQuery(command);
        }
    }
}
