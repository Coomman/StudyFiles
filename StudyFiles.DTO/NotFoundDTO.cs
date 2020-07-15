namespace StudyFiles.DTO
{
    public class NotFoundDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; } = "No matches found";

        public int SubType { get; } = 6;
    }
}
