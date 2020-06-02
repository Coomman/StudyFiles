using System;

namespace StudyFiles.DTO
{
    public class NullDTO : IEntityDTO
    {
        public Guid ID { get; }
        public string Name { get; }
    }
}
