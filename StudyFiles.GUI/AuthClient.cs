using System;
using System.IO;
using System.Configuration;
using System.Xml.Serialization;

using StudyFiles.DTO.Auth;
using StudyFiles.GUI.ServiceProxies;

namespace StudyFiles.GUI
{
    public sealed class AuthClient
    {
        #region Singleton

        private static readonly Lazy<AuthClient> Singleton = new Lazy<AuthClient> (() => new AuthClient());

        public static AuthClient GetInstance => Singleton.Value;

        private AuthClient()
        {
            TryReadSession();
        }

        #endregion

        #region LoginDTO properties

        private LoginDTO _loginDto;
        private LoginDTO LoginDto
        {
            get => _loginDto;
            set
            {
                _loginDto = null;

                if ((value != null) && (_authService.ValidateToken(value.Id, value.Token)))
                    _loginDto = _authService.RestoreSession(value.Id);

                IsAuthenticated = _loginDto != null;
                TryUpdateSessionFile(value);
            }
        }

        public bool IsAuthenticated { get; private set; }

        public int Id
            => LoginDto?.Id ?? -1;

        public string UserName
            => LoginDto?.Login ?? "Need to login first";

        public string Token
            => LoginDto?.Token ?? "";

        #endregion

        private readonly AuthServiceProxy _authService = new AuthServiceProxy();

        private readonly string _sessionPath = ConfigurationManager.AppSettings["SessionFilePath"];

        private void TryUpdateSessionFile(LoginDTO dto)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(LoginDTO));
                using var fs = new FileStream(_sessionPath, FileMode.Create);
                serializer.Serialize(fs, dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // TODO: Add logger
            }
        }
        private void TryReadSession()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(LoginDTO));
                using var fs = new FileStream(_sessionPath, FileMode.OpenOrCreate);
                LoginDto = (LoginDTO) serializer.Deserialize(fs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool Register(string email, string login, string password)
        {
            return _authService.Register(email, login, password);
        }
        public bool Login(string login, string password)
        {
            LoginDto = _authService.Login(login, password);

            return IsAuthenticated;
        }
        public void Logout()
        {
            if (LoginDto == null)
                return;

            _authService.Logout(Id);

            LoginDto = null;
        }
    }
}
