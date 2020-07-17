namespace StudyFiles.DTO.Requests.Core
{
    public class FolderDataRequest : IDataRequest
    {
        public int Depth { get; set; }
        public int ID { get; set; }
        public string ModelName { get; set; }

        public override string ToString()
        {
            return $"Depth: {Depth} ID: {ID} ModelName: {ModelName}";
        }
    }
}
