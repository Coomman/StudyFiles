namespace StudyFiles.DTO
{
    public class UniversityDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 0;
    }
}
