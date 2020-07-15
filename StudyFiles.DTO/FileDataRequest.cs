namespace StudyFiles.DTO
{
    public class FileDataRequest : IDataRequest
    {
        public int ID { get; set; }
        public string ModelName { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }
    }
}
