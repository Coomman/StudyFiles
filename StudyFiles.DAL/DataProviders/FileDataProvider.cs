using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers;
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
        public static SearchResultDTO GetSearchResultDTO(FileInfo fileInfo)
        {
            var courseId = int.Parse(fileInfo.Directory.Name);

            var (path, breadCrumb) = GetBreadCrunch(courseId);

            return new SearchResultDTO(GetFileDTO(fileInfo, courseId), path, breadCrumb);
        }

        private static (string path, string breadCrumb) GetBreadCrunch(int courseId)
        {
            const string query = "Select CONCAT_WS('/', U.Name, F.Name, D.Name, C.Teacher) as BreadCrumb, " +
                                 "CONCAT_WS('\\', U.ID, F.ID, D.ID, C.ID) as Path " +
                                 "from University as U " +
                                 "inner join Faculty as F on(U.ID = F.UniversityID) " +
                                 "inner join Discipline as D on(F.ID = D.FacultyID) " +
                                 "inner join Course as C on(D.ID = C.DisciplineID) " +
                                 "Where C.ID = @id";
            
            var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = courseId});

            return DBHelper.GetItem(new PathMapper(), command);
        }
    }
}
