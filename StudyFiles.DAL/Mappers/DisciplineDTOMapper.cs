using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    internal class DisciplineDTOMapper : IMapper<DisciplineDTO>
    {
        public DisciplineDTO ReadItem(SqlDataReader dr)
        {
            return new DisciplineDTO(
                (int)dr["ID"],
                (string)dr["Name"],
                (int)dr["FacultyID"]);
        }
    }
}
