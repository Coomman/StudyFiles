namespace StudyFiles.DTO
{
    public class FolderDataRequest : IDataRequest
    {
        public int Depth { get; set; }
        public int ID { get; set; }
        public string ModelName { get; set; }
    }
}
