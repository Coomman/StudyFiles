using System.Data.SqlClient;
using StudyFiles.DTO.Catalog;

namespace StudyFiles.DAL.Mappers.Catalog
{
    internal class DisciplineDTOMapper : IMapper<DisciplineDTO>
    {
        public DisciplineDTO ReadItem(SqlDataReader dr)
        {
            return new DisciplineDTO
            {
                ID = (int) dr["ID"],
                InnerText = (string) dr["Name"],
                FacultyID = (int) dr["FacultyID"]
            };
        }
    }
}
