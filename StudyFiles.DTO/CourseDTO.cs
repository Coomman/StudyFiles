using System;

namespace StudyFiles.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid DisciplineID { get; set; }
    }
}
