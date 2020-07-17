namespace StudyFiles.DTO.Auth
{
    public class AuthDTO
    {
        public int Id { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] Salt { get; set; }
    }
}
