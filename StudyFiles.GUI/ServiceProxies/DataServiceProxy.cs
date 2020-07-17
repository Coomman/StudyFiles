using System.IO;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Collections.Generic;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using StudyFiles.DTO.Files;
using StudyFiles.DTO.Requests.Core;
using StudyFiles.DTO.Service;
using StudyFiles.Logging;

namespace StudyFiles.GUI.ServiceProxies
{
    public class DataServiceProxy
    {
        private static readonly ILogger Logger = LoggerFactory.CreateLoggerFor<AuthServiceProxy>();

        private readonly RestClient _client;

        public DataServiceProxy()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["DataServiceUrl"]);
            _client.UseJson();
            _client.UseNewtonsoftJson();
        }

        private static void CheckRequestResult(IRestResponse request)
        {
            if (request.IsSuccessful)
                return;

            var ex = new HttpRequestException("Error occured in DataService Client");
            Logger.Error(ex, request.ErrorMessage);
            throw ex;
        }

        private T GetRequest<T>(string requestQuery)
        {
            var request = new RestRequest(requestQuery);

            var result = _client.Get<T>(request);

            CheckRequestResult(result);

            return result.Data; // TODO: Add try-catch with logs
        }
        private T PostRequest<T>(string requestQuery, IDataRequest requestData)
        {
            var request = new RestRequest(requestQuery)
                .AddJsonBody(requestData);

            var result = _client.Post<T>(request);

            CheckRequestResult(result);

            return result.Data; // TODO: Add try-catch with logs
        }
        private void DeleteRequest(string requestQuery, IDataRequest requestData = null)
        {
            var request = new RestRequest(requestQuery);

            if (requestData != null)
                request.AddJsonBody(requestData);

            var result = _client.Delete(request);

            CheckRequestResult(result);
        }

        public List<IEntityDTO> GetFolderList(int depth, int id = -1)
        {
            return PostRequest<List<IEntityDTO>>("data/folders",
                new FolderDataRequest {Depth = depth, ID = id});
        }
        public List<IEntityDTO> GetFileList(string path, int courseID)
        {
            return PostRequest<List<IEntityDTO>>("data/files",
                new FileDataRequest() {Path = path, ID = courseID});
        }

        public FileViewDTO GetFile(string filePath, string extension)
        {
            var tempFile = Path.GetFullPath(Path.ChangeExtension(Path.GetTempFileName(), extension));

            var result = GetRequest<byte[]>($"data/download?filePath={filePath}");

            using (var fs = File.Create(tempFile))
                fs.Write(result);

            return new FileViewDTO {InnerText = tempFile};
        }

        public IEntityDTO AddNewFolder(int depth, string modelName, int parentID)
        {
            return PostRequest<IEntityDTO>("data/newFolder",
                new FolderDataRequest {Depth = depth, ModelName = modelName, ID = parentID});
        }
        public IEntityDTO UploadFile(byte[] data, string path, int courseID)
        {
            return PostRequest<IEntityDTO>("data/upload",
                new FileDataRequest() {Data = data, Path = path, ID = courseID});
        }

        public void DeleteFolder(int depth, string path, int id)
        {
            DeleteRequest("data/folder",
                new FolderDataRequest {Depth = depth, ID = id, ModelName = path});
        }
        public void DeleteFile(string filePath)
        {
            DeleteRequest($"data/file?filePath={filePath}");
        }

        public List<IEntityDTO> FindFiles(string path, string searchQuery)
        {
            var result =  PostRequest<List<IEntityDTO>>("data/search",
                new FileDataRequest {Path = path, ModelName = searchQuery});

            if (!result.Any())
                result.Add(new NotFoundDTO { InnerText = $"No files match \"{searchQuery}\" query" });

            return result;
        }
    }
}
