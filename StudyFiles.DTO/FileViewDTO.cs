namespace StudyFiles.DTO
{
    public class FileViewDTO : IEntityDTO
    {
        public int ID { get; }
        public string Name { get; }

        public FileViewDTO(string path)
        {
            Name = path;
        }
    }
}
