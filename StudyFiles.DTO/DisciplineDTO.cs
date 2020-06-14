using System;

namespace StudyFiles.DTO
{
    public class DisciplineDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }
        public int FacultyID { get; }

        public DisciplineDTO(int id, string name, int facultyID)
        {
            ID = id;
            Name = name;
            FacultyID = facultyID;
        }
    }
}
