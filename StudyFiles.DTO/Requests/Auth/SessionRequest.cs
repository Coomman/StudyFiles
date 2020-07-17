namespace StudyFiles.DTO.Requests.Auth
{
    public class SessionRequest : IAuthRequest
    {
        public int ID { get; set; }

        public string Token { get; set; }
    }
}
