using System;

namespace StudyFiles.DTO
{
    public interface IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
    }
}
