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
    public class FileRepository : IFileRepository
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
        private (string path, string breadCrumb) GetPath(int courseId)
        {
            var command = new SqlCommand(Queries.GetPath);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = courseId });

            return _dbHelper.GetItem(new PathMapper(), command);
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
            using (var fileStream = File.Create(filePath))
                fileStream.Write(data);

            return GetFileDTO(new FileInfo(filePath), courseId);
        }

        public FileDTO GetFileDTO (FileInfo fileInfo, int courseId)
        {
            return new FileDTO
            {
                InnerText = fileInfo.Name, CourseID = courseId, 
                CreationTime = fileInfo.CreationTimeUtc.ToString("MM/dd/yyyy h:mm"), 
                Extension = fileInfo.Extension,
                Size = ByteSizeToString(fileInfo.Length)
            };
        }
        public SearchResultDTO GetSearchResultDTO(FileInfo fileInfo, List<int> pageEntries, string storagePath)
        {
            var courseId = int.Parse(fileInfo.Directory.Name);

            var (path, breadCrumb) = GetPath(courseId);

            return new SearchResultDTO
            {
                FileInfo = GetFileDTO(fileInfo, courseId),
                Path = Path.Combine(storagePath, path),
                BreadCrumb = breadCrumb,
                PageEntries = pageEntries
            };
        }
    }
}
