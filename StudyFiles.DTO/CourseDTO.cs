namespace StudyFiles.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 3;
        public int DisciplineID { get; set; }
    }
}
