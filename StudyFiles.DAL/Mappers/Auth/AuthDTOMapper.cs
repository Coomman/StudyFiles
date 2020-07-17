using System.Data.SqlClient;
using StudyFiles.DTO.Auth;

namespace StudyFiles.DAL.Mappers.Auth
{
    public class AuthDTOMapper : IMapper<AuthDTO>
    {
        public AuthDTO ReadItem(SqlDataReader dr)
        {
            return new AuthDTO
            {
                Id = (int) dr["ID"],
                PassHash = (byte[]) dr["PassHash"],
                Salt = (byte[]) dr["salt"]
            };
        }
    }
}