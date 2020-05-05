using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class DisciplineDTOMapper : IMapper<DisciplineDTO>
    {
        public DisciplineDTO ReadItem(SqlDataReader dr)
        {
            return new DisciplineDTO()
            {
                ID = (int)dr["ID"],
                Name = (string)dr["Name"],
                FacultyID = (int)dr["FacultyID"]
            };
        }
    }
}
