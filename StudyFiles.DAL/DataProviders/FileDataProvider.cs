using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using StudyFiles.DTO;

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
                .Select(file => new FileDTO(0, file.Name, ByteSizeToString(file.Length), courseId, 
                    file.CreationTime.ToString("MM/dd/yyyy h:mm")));
        }

        public static FileDTO UploadFile(DirectoryInfo dir, int courseId, string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            File.Copy(filePath, Path.Combine(dir.FullName, fileInfo.Name));

            return new FileDTO(0, fileInfo.Name, ByteSizeToString(fileInfo.Length),
                courseId, fileInfo.CreationTimeUtc.ToString("MM/dd/yyyy h:mm"));
        }
    }
}
