using System;

namespace StudyFiles.DTO
{
    public class FileDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
        public string Size { get; }
        public Guid CourseID { get; }
        public string CreationTime { get; }

        public FileDTO(Guid id, string name, string size, Guid courseID, string creationTime)
        {
            ID = id;
            Name = name;
            Size = size;
            CourseID = courseID;
            CreationTime = creationTime;
        }
    }
}
