using System;

namespace StudyFiles.DTO
{
    public class DisciplineDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
        public Guid FacultyID { get; }

        public DisciplineDTO(Guid id, string name, Guid facultyID)
        {
            ID = id;
            Name = name;
            FacultyID = facultyID;
        }
    }
}
