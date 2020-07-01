using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    internal class CourseDTOMapper : IMapper<CourseDTO>
    {
        public CourseDTO ReadItem(SqlDataReader dr)
        {
            return new CourseDTO((int) dr["ID"],
                (string) dr["Teacher"],
                (int) dr["DisciplineID"]);
        }
    }
}
