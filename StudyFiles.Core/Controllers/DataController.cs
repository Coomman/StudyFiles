using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StudyFiles.DTO;

namespace StudyFiles.Core.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : ControllerBase
    {
        private readonly IDataSupplier _dataSupplier;

        public DataController(IDataSupplier dataSupplier)
        {
            _dataSupplier = dataSupplier;
        }

        [HttpPost]
        [Route("folders")]
        public IEnumerable<IEntityDTO> GetFolderList(FolderDataRequest request)
        {
            return _dataSupplier.GetFolderList(request.Depth, request.ID);
        }

        [HttpPost]
        [Route("files")]
        public IEnumerable<IEntityDTO> GetFileList(FileDataRequest request)
        {
            return _dataSupplier.GetFileList(request.ID, request.Path);
        }

        [HttpPost]
        [Route("newFolder")]
        public IEntityDTO AddFolder(FolderDataRequest request)
        {
            return _dataSupplier.AddNewFolder(request.Depth, request.ModelName, request.ID);
        }

        [HttpPost]
        [Route("upload")]
        public IEntityDTO AddNewModel(FileDataRequest request)
        {
            return _dataSupplier.UploadFile(request.Data, request.Path, request.ID);
        }

        [HttpDelete]
        [Route("deleteModel")]
        public void DeleteModel(int depth, int id)
        {
            
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<IEntityDTO> FindFiles(int depth, string searchQuery)
        {
            var result = _dataSupplier.FindFiles(depth, searchQuery).ToList();

            if (!result.Any())
                result.Add(new NotFoundDTO { InnerText = $"No files match \"{searchQuery}\" query" });

            return result;
        }

        [HttpGet]
        [Route("download")]
        public byte[] GetFile(string filePath)
        {
            return _dataSupplier.GetFile(filePath);
        }
    }
}
