namespace StudyFiles.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; }
        public int DisciplineID { get; }

        public CourseDTO(int id, string teacher, int disciplineID)
        {
            ID = id;
            InnerText = teacher;
            DisciplineID = disciplineID;
        }
    }
}
