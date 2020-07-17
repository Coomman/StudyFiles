using System.Data.SqlClient;
using StudyFiles.DTO.Auth;

namespace StudyFiles.DAL.Mappers.Auth
{
    public class LoginDTOMapper : IMapper<LoginDTO>
    {
        public LoginDTO ReadItem(SqlDataReader dr)
        {
            return new LoginDTO
            {
                Id = (int) dr["ID"],
                Login = (string) dr["Login"],
                Token = (string) dr["Token"]
            };
        }
    }
}
