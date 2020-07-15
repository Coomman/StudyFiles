namespace StudyFiles.DTO
{
    public class FacultyDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 1;
        public int UniversityID { get; set; }
    }
}
