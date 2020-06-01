using System;

namespace StudyFiles.DTO
{
    public class UniversityDTO : IEntityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
