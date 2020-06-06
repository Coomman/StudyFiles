using System;

namespace StudyFiles.DTO
{
    public class FileViewDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }

        public string Content { get; }

        public FileViewDTO(string content)
        {
            Content = content;
        }
    }
}
