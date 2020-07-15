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
        [HttpPost]
        [Route("folders")]
        public IEnumerable<IEntityDTO> GetModelsList(FolderDataRequest request)
        {
            return DataSupplier.GetModelsList(request.Depth, request.ID);
        }

        [HttpPost]
        [Route("files")]
        public IEnumerable<IEntityDTO> GetFileList(FileDataRequest request)
        {
            return DataSupplier.GetFilesList(request.ID, request.Path);
        }

        [HttpPost]
        [Route("newFolder")]
        public IEntityDTO AddFolder(FolderDataRequest request)
        {
            return DataSupplier.AddNewModel(request.Depth, request.ModelName, request.ID);
        }

        [HttpPost]
        [Route("upload")]
        public IEntityDTO AddNewModel(FileDataRequest request)
        {
            return DataSupplier.UploadFile(request.Data, request.Path, request.ID);
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
            var result =  DataSupplier.FindFiles(depth, searchQuery).ToList();

            if (!result.Any())
                result.Add(new NotFoundDTO { InnerText = $"No files match \"{searchQuery}\" query" });

            return result;
        }

        [HttpGet]
        [Route("download")]
        public byte[] ReadFile(string filePath)
        {
            return DataSupplier.ReadFile(filePath);
        }
    }
}
