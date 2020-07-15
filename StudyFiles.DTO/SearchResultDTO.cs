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
        public string Size => FileInfo.Size;
        public int CourseID => FileInfo.CourseID;
        public string CreationTime => FileInfo.CreationTime;

        public string Path { get; set; }
        public string BreadCrumb { get; set; }
        public List<int> PageEntries { get; set; }
    }
}
