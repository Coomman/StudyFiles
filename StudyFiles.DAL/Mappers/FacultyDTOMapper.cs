using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    internal class FacultyDTOMapper : IMapper<FacultyDTO>
    {
        public FacultyDTO ReadItem(SqlDataReader dr)
        {
            return new FacultyDTO((int) dr["ID"],
                (string) dr["Name"],
                (int) dr["UniversityID"]);
        }
    }
}
