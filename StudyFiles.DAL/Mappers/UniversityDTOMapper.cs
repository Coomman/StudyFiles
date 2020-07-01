using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    internal class UniversityDTOMapper : IMapper<UniversityDTO>
    {
        public UniversityDTO ReadItem(SqlDataReader dr)
        {
            return new UniversityDTO(
                (int) dr["ID"],
                (string) dr["Name"]);
        }
    }
}
