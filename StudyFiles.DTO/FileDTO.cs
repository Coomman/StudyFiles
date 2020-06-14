using System;

namespace StudyFiles.DTO
{
    public class FileDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }
        public string Size { get; }
        public int CourseID { get; }
        public string CreationTime { get; }

        public FileDTO(int id, string name, string size, int courseID, string creationTime)
        {
            ID = id;
            Name = name;
            Size = size;
            CourseID = courseID;
            CreationTime = creationTime;
        }
    }
}
