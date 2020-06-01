using System;

namespace StudyFiles.DTO
{
    public class FacultyDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
        public Guid UniversityID { get; }

        public FacultyDTO(Guid id, string name, Guid universityID)
        {
            ID = id;
            Name = name;
            UniversityID = universityID;
        }
    }
}
