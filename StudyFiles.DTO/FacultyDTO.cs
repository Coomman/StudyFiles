using System;

namespace StudyFiles.DTO
{
    public class FacultyDTO : IEntityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid UniversityID { get; set; }
    }
}
