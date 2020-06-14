using System;

namespace StudyFiles.DTO
{
    public class UniversityDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }

        public UniversityDTO(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
