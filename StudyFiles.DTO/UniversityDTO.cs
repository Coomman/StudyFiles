using System;

namespace StudyFiles.DTO
{
    public class UniversityDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }

        public UniversityDTO(Guid id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
