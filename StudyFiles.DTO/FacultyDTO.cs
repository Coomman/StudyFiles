namespace StudyFiles.DTO
{
    public class FacultyDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; }
        public int UniversityID { get; }

        public FacultyDTO(int id, string name, int universityID)
        {
            ID = id;
            InnerText = name;
            UniversityID = universityID;
        }
    }
}
