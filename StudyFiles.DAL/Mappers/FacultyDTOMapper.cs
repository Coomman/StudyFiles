using System;
using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class FacultyDTOMapper : IMapper<FacultyDTO>
    {
        public FacultyDTO ReadItem(SqlDataReader dr)
        {
            return new FacultyDTO((Guid) dr["ID"],
                (string) dr["Name"],
                (Guid) dr["UniversityID"]);
        }
    }
}
