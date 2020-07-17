using StudyFiles.DTO.Service;

namespace StudyFiles.DTO.Catalog
{
    public class DisciplineDTO : IEntityDTO
    {
        public int ID { get; set; }
        public string InnerText { get; set; }
        public int SubType { get; } = 2;
        public int FacultyID { get; set; }
    }
}
