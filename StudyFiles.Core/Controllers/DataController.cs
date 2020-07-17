using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StudyFiles.Core.Suppliers;
using StudyFiles.DAL;
using StudyFiles.DTO.Requests.Core;
using StudyFiles.DTO.Service;
using StudyFiles.Logging;

namespace StudyFiles.Core.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : ControllerBase
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<DBHelper>();

        private readonly IDataSupplier _dataSupplier;

        public DataController(IDataSupplier dataSupplier)
        {
            _dataSupplier = dataSupplier;
        }

        [HttpPost]
        [Route("folders")]
        public IEnumerable<IEntityDTO> GetFolderList(FolderDataRequest request)
        {
            try
            {
                return _dataSupplier.GetFolderList(request.Depth, request.ID);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpPost]
        [Route("files")]
        public IEnumerable<IEntityDTO> GetFileList(FileDataRequest request)
        {
            try
            {
                return _dataSupplier.GetFileList(request.ID, request.Path);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpPost]
        [Route("newFolder")]
        public IEntityDTO AddFolder(FolderDataRequest request)
        {
            try
            {
                return _dataSupplier.AddNewFolder(request.Depth, request.ModelName, request.ID);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpPost]
        [Route("upload")]
        public IEntityDTO AddNewModel(FileDataRequest request)
        {
            try
            {
                return _dataSupplier.UploadFile(request.Data, request.Path, request.ID);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpDelete]
        [Route("folder")]
        public void DeleteFolder(FolderDataRequest request)
        {
            try
            {
                _dataSupplier.DeleteFolder(request.Depth, request.ModelName, request.ID);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpDelete]
        [Route("file")]
        public void DeleteFile(string filePath)
        {
            try
            {
                _dataSupplier.DeleteFile(filePath);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With filePath: {filePath}");
                throw;
            }
        }

        [HttpPost]
        [Route("search")]
        public IEnumerable<IEntityDTO> FindFiles(FileDataRequest request)
        {
            try
            {
                return _dataSupplier.FindFiles(request.Path, request.ModelName);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With request: {request}");
                throw;
            }
        }

        [HttpGet]
        [Route("download")]
        public byte[] GetFile(string filePath)
        {
            try
            {
                return _dataSupplier.GetFile(filePath);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"With filePath: {filePath}");
                throw;
            }
        }
    }
}
