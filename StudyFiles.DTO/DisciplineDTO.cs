using System;

namespace StudyFiles.DTO
{
    public class DisciplineDTO : IEntityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid FacultyID { get; set; }
    }
}
