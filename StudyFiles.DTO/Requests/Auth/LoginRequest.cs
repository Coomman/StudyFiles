namespace StudyFiles.DTO.Requests.Auth
{
    public class LoginRequest : IAuthRequest
    {
        public string Email { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
