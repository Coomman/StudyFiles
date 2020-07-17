using System.Collections.Generic;
using StudyFiles.DTO.Service;

namespace StudyFiles.Core.Suppliers
{
    public interface IDataSupplier
    {
        public List<IEntityDTO> GetFolderList(int depth, int id = -1);
        public IEnumerable<IEntityDTO> GetFileList(int courseID, string filePath);

        public IEntityDTO AddNewFolder(int depth, string modelName, int parentId);
        public IEntityDTO UploadFile(byte[] file, string filePath, int courseID);

        public void DeleteFolder(int depth, string path, int id);
        public void DeleteFile(string filePath);

        public byte[] GetFile(string filePath);

        public IEnumerable<IEntityDTO> FindFiles(string path, string searchQuery);
    }
}
