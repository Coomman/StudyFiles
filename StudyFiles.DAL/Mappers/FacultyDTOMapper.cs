using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    internal class FacultyDTOMapper : IMapper<FacultyDTO>
    {
        public FacultyDTO ReadItem(SqlDataReader dr)
        {
            return new FacultyDTO
            {
                ID = (int) dr["ID"],
                InnerText = (string) dr["Name"],
                UniversityID = (int) dr["UniversityID"]
            };
        }
    }
}
