using StudyFiles.DTO.Service;

namespace StudyFiles.DTO.Files
{
    public class FileViewDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 5;
    }
}
