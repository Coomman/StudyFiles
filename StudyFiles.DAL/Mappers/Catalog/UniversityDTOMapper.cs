using System.Data.SqlClient;
using StudyFiles.DTO.Catalog;

namespace StudyFiles.DAL.Mappers.Catalog
{
    internal class UniversityDTOMapper : IMapper<UniversityDTO>
    {
        public UniversityDTO ReadItem(SqlDataReader dr)
        {
            return new UniversityDTO
            {
                ID = (int) dr["ID"],
                InnerText = (string) dr["Name"]
            };
        }
    }
}
