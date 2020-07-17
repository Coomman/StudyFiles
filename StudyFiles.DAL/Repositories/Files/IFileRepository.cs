using System.Collections.Generic;
using System.IO;
using StudyFiles.DTO.Files;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Files
{
    public interface IFileRepository
    {
        public IEnumerable<IEntityDTO> GetFiles(string dir, int courseId);

        public byte[] GetFile(string filePath);
        public FileDTO UploadFile(byte[] data, string filePath, int courseId);

        public void DeleteFolder(string path);
        public void DeleteFile(string filePath);

        public bool InFileSearch(string filePath, string searchQuery);

        public SearchResultDTO GetSearchResultDTO(FileInfo fileInfo);
    }
}
