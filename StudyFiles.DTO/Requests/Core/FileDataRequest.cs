namespace StudyFiles.DTO.Requests.Core
{
    public class FileDataRequest : IDataRequest
    {
        public int ID { get; set; }
        public string ModelName { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return $"ID: {ID} ModelName: {ModelName} Path: {Path}";
        }
    }
}
