namespace StudyFiles.DTO
{
    public class DisciplineDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; }
        public int FacultyID { get; }

        public DisciplineDTO(int id, string name, int facultyID)
        {
            ID = id;
            InnerText = name;
            FacultyID = facultyID;
        }
    }
}
