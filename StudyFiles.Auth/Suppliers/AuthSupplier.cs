using System.Linq;
using StudyFiles.DAL.Repositories.Auth;
using StudyFiles.DTO.Auth;

namespace StudyFiles.Auth.Suppliers
{
    public class AuthSupplier : IAuthSupplier
    {
        private readonly IAuthRepository _authRep;

        public AuthSupplier(IAuthRepository authRep)
        {
            _authRep = authRep;
        }


        public bool Register(string email, string login, string password)
        {
            if (_authRep.VerifyUserExistence(login))
                return false;

            var salt = CryptoService.GenerateSalt(login);

            _authRep.CreateAccount(email, login, CryptoService.ComputeHash(password, salt), salt);

            return true;
        }

        public LoginDTO Authenticate(string login, string password)
        {
            if (!_authRep.VerifyUserExistence(login))
                return null;

            var userData = _authRep.GetUserData(login);

            if (!userData.PassHash.SequenceEqual(CryptoService.ComputeHash(password, userData.Salt)))
                return null;

            var token = CryptoService.GenerateToken();
            _authRep.StartSession(userData.Id, token);

            return new LoginDTO {Id = userData.Id, Login = login, Token = token};

        }

        public LoginDTO RestoreSession(int id)
        {
            return _authRep.RestoreSession(id);
        }

        public void Logout(int id)
        {
            _authRep.EndSession(id);
        }

        public bool ValidateToken(int id, string token)
        {
            return _authRep.ValidateToken(id, token);
        }
    }
}
