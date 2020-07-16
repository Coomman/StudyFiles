using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public interface ICourseRepository
    {
        public IEnumerable<CourseDTO> GetCourses(int disciplineID);

        public IEntityDTO AddCourse(string teacherName, int disciplineID);
    }
}
