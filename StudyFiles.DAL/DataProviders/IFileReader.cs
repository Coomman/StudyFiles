namespace StudyFiles.DAL.DataProviders
{
    public interface IFileReader
    {
        public bool FileSearch(string fileExtension, string filePath, string searchQuery);
    }
}
