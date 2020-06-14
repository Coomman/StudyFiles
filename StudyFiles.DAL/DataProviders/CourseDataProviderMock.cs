using System;
using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class CourseDataProviderMock
    {
        private static int NextID { get; set; } = 2;

        private static readonly List<CourseDTO> Courses = new List<CourseDTO>
        {
            new CourseDTO( 1, "Bulanova Nina", 1)
        };

        public static List<CourseDTO> GetCourses(int disciplineID)
        {
            return Courses.Where(c => c.DisciplineID == disciplineID).ToList();
        }
        public static IEntityDTO AddCourse(string teacherName, int disciplineID)
        {
            var course = new CourseDTO(NextID++, teacherName, disciplineID);

            Courses.Add(course);

            return course;
        }
    }
}
