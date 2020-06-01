using System;
using System.Data.SqlClient;
using StudyFiles.DTO;

namespace StudyFiles.DAL.Mappers
{
    public class DisciplineDTOMapper : IMapper<DisciplineDTO>
    {
        public DisciplineDTO ReadItem(SqlDataReader dr)
        {
            return new DisciplineDTO()
            {
                ID = (Guid)dr["ID"],
                Name = (string)dr["Name"],
                FacultyID = (Guid)dr["FacultyID"]
            };
        }
    }
}
