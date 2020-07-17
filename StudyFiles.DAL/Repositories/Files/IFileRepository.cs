using System.Collections.Generic;
using System.IO;
using StudyFiles.DTO.Files;

namespace StudyFiles.DAL.Repositories.Files
{
    public interface IFileRepository
    {
        public IEnumerable<FileDTO> GetFiles(DirectoryInfo dir, int courseId);

        public byte[] GetFile(string filePath);
        public FileDTO UploadFile(byte[] data, string filePath, int courseId);

        public void DeleteFolder(string path);
        public void DeleteFile(string filePath);

        public bool InFileSearch(string filePath, string searchQuery);

        public FileDTO GetFileDTO(FileInfo fileInfo, int courseId);
        public SearchResultDTO GetSearchResultDTO(FileInfo fileInfo);
    }
}
