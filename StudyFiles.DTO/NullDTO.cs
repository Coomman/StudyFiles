﻿namespace StudyFiles.DTO
{
    public class NullDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 7;
    }
}
