using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class CourseDataProviderMock
    {
        private static readonly List<CourseDTO> Courses = new List<CourseDTO>
        {
            new CourseDTO {ID = 1, Teacher = "Bulanova Nina", DisciplineID = 3}
        };

        public static List<CourseDTO> GetCourses(int disciplineID)
        {
            return Courses.Where(c => c.DisciplineID == disciplineID).ToList();
        }
        public static void AddCourse(CourseDTO course)
        {
            course.ID = Courses.Last().ID + 1;
            Courses.Add(course);
        }
    }
}
