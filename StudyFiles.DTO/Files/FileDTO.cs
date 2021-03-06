﻿using StudyFiles.DTO.Service;

namespace StudyFiles.DTO.Files
{
    public class FileDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 4;
        public int CourseID { get; set; }

        public string Path { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
        public string CreationTime { get; set; }
    }
}
