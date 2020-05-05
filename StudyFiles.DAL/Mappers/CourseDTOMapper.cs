using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class CourseDTOMapper : IMapper<CourseDTO>
    {
        public CourseDTO ReadItem(SqlDataReader dr)
        {
            return new CourseDTO()
            {
                ID = (int)dr["ID"],
                Teacher = (string)dr["Teacher"],
                DisciplineID = (int)dr["DisciplineID"]
            };
        }
    }
}
