namespace StudyFiles.DTO.Files
{
    public class SearchResultDTO : FileDTO
    {
        public new int SubType { get; } = 8;

        public string BreadCrumb { get; set; }
    }
}
