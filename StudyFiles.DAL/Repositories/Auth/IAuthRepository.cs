using StudyFiles.DTO.Auth;

namespace StudyFiles.DAL.Repositories.Auth
{
    public interface IAuthRepository
    {
        public void CreateAccount(string email, string login, byte[] passHash, byte[] salt);

        public bool VerifyUserExistence(string login);
        public AuthDTO GetUserData(string login);

        public void StartSession(int id, string token);
        public bool ValidateToken(int id, string token);
        public LoginDTO RestoreSession(int id);
        public  void EndSession(int id);
    }
}
