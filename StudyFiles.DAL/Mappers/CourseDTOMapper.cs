using System;
using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class CourseDTOMapper : IMapper<CourseDTO>
    {
        public CourseDTO ReadItem(SqlDataReader dr)
        {
            return new CourseDTO((int)dr["ID"], 
                (string)dr["Name"],
                (int)dr["DisciplineID"]);
        }
    }
}
