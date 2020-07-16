using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using StudyFiles.DTO;
using StudyFiles.DAL.Mappers;

namespace StudyFiles.DAL.DataProviders
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

        public IEnumerable<FileDTO> GetFiles(DirectoryInfo dir, int courseId)
        {
            return dir.GetFiles()
                .Select(fileInfo => GetFileDTO(fileInfo, courseId));
        }

        public byte[] GetFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
        public FileDTO UploadFile(byte[] data, string filePath, int courseId)
        {
            var newFilePath = _fileReader.SaveFile(filePath, data);

            return GetFileDTO(new FileInfo(newFilePath), courseId);
        }
        public bool InFileSearch(string filePath, string searchQuery)
        {
            return _fileReader.FileSearch(filePath, searchQuery);
        }

        public FileDTO GetFileDTO (FileInfo fileInfo, int courseId)
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

            return new SearchResultDTO
            {
                FileInfo = GetFileDTO(fileInfo, courseId),
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
