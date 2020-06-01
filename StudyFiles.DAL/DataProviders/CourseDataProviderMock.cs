using System;
using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class CourseDataProviderMock
    {
        private static readonly List<CourseDTO> Courses = new List<CourseDTO>
        {
            new CourseDTO( Guid.Parse("559cb333-b30a-4a1c-b7dd-37caaca5423f"), "Bulanova Nina",
                Guid.Parse("abecf0cc-ce2c-4667-9a2c-14bffe36bd7e"))
        };

        public static List<CourseDTO> GetCourses(Guid disciplineID)
        {
            return Courses.Where(c => c.DisciplineID == disciplineID).ToList();
        }
        public static void AddCourse(CourseDTO course)
        {
            Courses.Add(course);
        }
    }
}
