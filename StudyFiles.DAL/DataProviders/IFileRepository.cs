using System.Collections.Generic;
using System.IO;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public interface IFileRepository
    {
        public IEnumerable<FileDTO> GetFiles(DirectoryInfo dir, int courseId);

        public byte[] GetFile(string filePath);
        public FileDTO UploadFile(byte[] data, string filePath, int courseId);

        public FileDTO GetFileDTO(FileInfo fileInfo, int courseId);
        public SearchResultDTO GetSearchResultDTO(FileInfo fileInfo, List<int> pageEntries, string storagePath);
    }
}
