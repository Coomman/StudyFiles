using System.Data.SqlClient;
using StudyFiles.DTO.Catalog;

namespace StudyFiles.DAL.Mappers.Catalog
{
    internal class CourseDTOMapper : IMapper<CourseDTO>
    {
        public CourseDTO ReadItem(SqlDataReader dr)
        {
            return new CourseDTO
            {
                ID = (int) dr["ID"],
                InnerText = (string) dr["Teacher"],
                DisciplineID = (int) dr["DisciplineID"]
            };
        }
    }
}
