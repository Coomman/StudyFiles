using System;

namespace StudyFiles.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
        public Guid DisciplineID { get; }

        public CourseDTO(Guid id, string teacher, Guid disciplineID)
        {
            ID =id;
            Name = teacher;
            DisciplineID = disciplineID;
        }
    }
}
