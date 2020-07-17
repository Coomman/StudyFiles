using System.Collections.Generic;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Catalog
{
    public interface ICourseRepository
    {
        public IEnumerable<CourseDTO> GetCourses(int disciplineID);

        public IEntityDTO AddCourse(string teacherName, int disciplineID);

        public void DeleteCourse(int id);
    }
}
