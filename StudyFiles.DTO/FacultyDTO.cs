using System;

namespace StudyFiles.DTO
{
    public class FacultyDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }
        public int UniversityID { get; }

        public FacultyDTO(int id, string name, int universityID)
        {
            ID = id;
            Name = name;
            UniversityID = universityID;
        }
    }
}
