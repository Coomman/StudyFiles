using System.Configuration;
using System.Net.Http;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using StudyFiles.DTO.Auth;
using StudyFiles.DTO.Requests.Auth;
using StudyFiles.Logging;

namespace StudyFiles.GUI.ServiceProxies
{
    public class AuthServiceProxy
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<AuthServiceProxy>();

        private readonly RestClient _client;

        public AuthServiceProxy()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["AuthServiceUrl"]);
            _client.UseJson();
            _client.UseNewtonsoftJson();
        }

        private static void CheckRequestResult(IRestResponse request)
        {
            if (request.IsSuccessful)
                return;

            var ex = new HttpRequestException($"Error occured in AuthService Client: {request.ErrorMessage}");
            Logger.Error(ex, request.ErrorMessage);
            throw ex;
        }

        private T GetRequest<T>(string requestQuery)
        {
            var request = new RestRequest(requestQuery);

            var result = _client.Get<T>(request);

            CheckRequestResult(result);

            return result.Data; // TODO: Add try-catch with logs
        }
        private T PostRequest<T>(string requestQuery, IAuthRequest requestData)
        {
            var request = new RestRequest(requestQuery)
                .AddJsonBody(requestData);

            var result = _client.Post<T>(request);

            CheckRequestResult(result);

            return result.Data; // TODO: Add try-catch with logs
        }
        private void PutRequest(string requestQuery)
        {
            var request = new RestRequest(requestQuery);

            var result = _client.Put(request);

            CheckRequestResult(result);
        }

        public bool Register(string email, string login, string password)
        {
            return PostRequest<bool>("auth/register",
                new LoginRequest {Email = email, Login = login, Password = password});
        }
        public LoginDTO Login(string login, string password)
        {
            return PostRequest<LoginDTO>("auth/login",
                new LoginRequest {Login = login, Password = password});
        }

        public LoginDTO RestoreSession(int id)
        {
            return GetRequest<LoginDTO>($"auth/restoreSession?id={id}");
        }
        public void Logout(int id)
        {
            PutRequest($"auth/logout?id={id}");
        }
        public bool ValidateToken(int id, string token)
        {
            return PostRequest<bool>("auth/validate",
                new SessionRequest {ID = id, Token = token});
        }
    }
}
