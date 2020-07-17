using StudyFiles.DTO.Auth;

namespace StudyFiles.Auth.Suppliers
{
    public interface IAuthSupplier
    {
        public bool Register(string email, string login, string password);

        public LoginDTO Authenticate(string login, string password);

        public LoginDTO RestoreSession(int id);

        public void Logout(int id);

        public bool ValidateToken(int id, string token);
    }
}
