namespace StudyFiles.DTO
{
    public class NotFoundDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; } = "No matches found";

        public NotFoundDTO(string notFoundText = null)
        {
            if (InnerText != null)
                InnerText = notFoundText;
        }
    }
}
