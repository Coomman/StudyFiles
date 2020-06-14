using System;

namespace StudyFiles.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }
        public int DisciplineID { get; }

        public CourseDTO(int id, string teacher, int disciplineID)
        {
            ID = id;
            Name = teacher;
            DisciplineID = disciplineID;
        }
    }
}
