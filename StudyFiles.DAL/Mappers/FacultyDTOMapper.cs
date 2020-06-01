using System;
using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class FacultyDTOMapper : IMapper<FacultyDTO>
    {
        public FacultyDTO ReadItem(SqlDataReader dr)
        {
            return new FacultyDTO
            {
                ID = (Guid) dr["ID"],
                Name = (string) dr["Name"],
                UniversityID = (Guid) dr["UniversityID"]
            };
        }
    }
}
