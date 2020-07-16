using System.Collections.Generic;

namespace StudyFiles.DTO
{
    public class SearchResultDTO : IEntityDTO
    {
        public FileDTO FileInfo { get; set; }

        public int ID
        {
            get => FileInfo.ID;
            set => FileInfo.ID = value;
        }
        public string InnerText
        {
            get => FileInfo.InnerText;
            set => FileInfo.InnerText = value;
        }
        public int SubType { get; } = 8;

        public string Path => FileInfo.Path;
        public string Size => FileInfo.Size;
        public int CourseID => FileInfo.CourseID;
        public string CreationTime => FileInfo.CreationTime;
        public string Extension => FileInfo.Extension;

        public string BreadCrumb { get; set; }
    }
}
