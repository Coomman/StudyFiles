namespace StudyFiles.DTO
{
    public class NotFoundDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; }

        public NotFoundDTO(string notFoundText = null)
        {
            InnerText = notFoundText ?? "No matches found";
        }
    }
}
