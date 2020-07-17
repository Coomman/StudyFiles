using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

using StudyFiles.DTO.Files;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Files
{
    public sealed class FileRepository : IFileRepository
    {
        private readonly IDBHelper _dbHelper;
        private readonly IFileReader _fileReader;

        public FileRepository(IDBHelper dbHelper, IFileReader fileReader)
        {
            _dbHelper = dbHelper;
            _fileReader = fileReader;
        }

        private static string ByteSizeToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB" };

            if (byteCount == 0)
                return "0 " + suf[0];

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }

        public IEnumerable<IEntityDTO> GetFiles(string dir, int courseId)
        {
            if(!Directory.Exists(dir))
                return new[] {new NotFoundDTO()};

            return new DirectoryInfo(dir).GetFiles()
                .Select(fileInfo => GetFileDTO(fileInfo, courseId));
        }

        public byte[] GetFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
        public FileDTO UploadFile(byte[] data, string filePath, int courseId)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var newFilePath = _fileReader.SaveFile(filePath, data);

            return GetFileDTO(new FileInfo(newFilePath), courseId);
        }

        public void DeleteFolder(string path)
        {
            if(Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public void DeleteFile(string filePath)
        {
            if(File.Exists(filePath))
                File.Delete(filePath);
        }

        public bool InFileSearch(string filePath, string searchQuery)
        {
            return _fileReader.FileSearch(filePath, searchQuery);
        }

        private static FileDTO GetFileDTO (FileInfo fileInfo, int courseId)
        {
            return new FileDTO
            {
                InnerText = fileInfo.Name, CourseID = courseId, 
                Path = fileInfo.FullName,
                CreationTime = fileInfo.CreationTimeUtc.ToString("MM/dd/yyyy H:mm"), 
                Extension = fileInfo.Extension,
                Size = ByteSizeToString(fileInfo.Length)
            };
        }
        public SearchResultDTO GetSearchResultDTO(FileInfo fileInfo)
        {
            var courseId = int.Parse(fileInfo.Directory.Name);
            var fileDto = GetFileDTO(fileInfo, courseId);

            return new SearchResultDTO
            {
                InnerText = fileDto.InnerText, CourseID = fileDto.CourseID,
                Path = fileDto.Path, CreationTime = fileDto.CreationTime,
                Extension = fileDto.Extension, Size = fileDto.Size,
                BreadCrumb = GetBreadCrumb(courseId)
            };
        }

        private string GetBreadCrumb(int courseId)
        {
            var command = new SqlCommand(Queries.GetBreadCrumb);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = courseId });

            return _dbHelper.ExecuteScalar<string>(command);
        }
    }
}
