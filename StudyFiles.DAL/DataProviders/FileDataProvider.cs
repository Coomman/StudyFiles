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
    public static class FileDataProvider
    {
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

        public static IEnumerable<FileDTO> GetFiles(DirectoryInfo dir, int courseId)
        {
            return dir.GetFiles()
                .Select(fileInfo => GetFileDTO(fileInfo, courseId));
        }

        public static FileDTO UploadFile(DirectoryInfo dir, int courseId, string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            var destPath = Path.Combine(dir.FullName, fileInfo.Name);

            File.Copy(filePath, destPath);

            return GetFileDTO(new FileInfo(destPath), courseId);
        }

        public static FileDTO GetFileDTO (FileInfo fileInfo, int courseId)
        {
            return new FileDTO(-1, fileInfo.Name, ByteSizeToString(fileInfo.Length), courseId,
                fileInfo.CreationTimeUtc.ToString("MM/dd/yyyy h:mm"));
        }
        public static SearchResultDTO GetSearchResultDTO(FileInfo fileInfo, List<int> pageEntries, string storagePath)
        {
            var courseId = int.Parse(fileInfo.Directory.Name);

            var (path, breadCrumb) = GetPath(courseId);

            return new SearchResultDTO(GetFileDTO(fileInfo, courseId), Path.Combine(storagePath, path), breadCrumb, pageEntries);
        }

        private static (string path, string breadCrumb) GetPath(int courseId)
        {
            var command = new SqlCommand(Queries.GetPath);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = courseId});

            return DBHelper.GetItem(new PathMapper(), command);
        }
    }
}
