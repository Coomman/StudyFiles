using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public static IEnumerable<FileDTO> GetFiles(DirectoryInfo dir)
        {
            return dir.GetFiles()
                .Select(file => new FileDTO(Guid.NewGuid(), file.Name, ByteSizeToString(file.Length), Guid.Empty, 
                    file.CreationTime.ToString("MM/dd/yyyy h:mm tt")));
        }

        public static FileDTO UploadFile(DirectoryInfo dir, string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            File.Copy(filePath, Path.Combine(dir.FullName, fileInfo.Name));

            return new FileDTO(Guid.NewGuid(), fileInfo.Name, ByteSizeToString(fileInfo.Length),
                Guid.Empty, fileInfo.CreationTimeUtc.ToString("MM/dd/yyyy h:mm"));
        }
    }
}
