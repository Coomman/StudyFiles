using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudyFiles.DAL.DataProviders;
using StudyFiles.DTO;

namespace StudyFiles.Core
{
    public static class DataSupplier
    {
        // Get from config file
        private const string StoragePath = @"D:\Git\StudyFiles\StudyFiles.Core\res\";

        static DataSupplier()
        {
            GetCmd = new Dictionary<int, Func<int, IEnumerable<IEntityDTO>>>
            {
                [0] = id => GetUniversities(),
                [1] = GetFaculties,
                [2] = GetDisciplines,
                [3] = GetCourses
            };

            AddCmd = new Dictionary<int, Func<string, int, IEntityDTO>>
            {
                [0] = (modelName, parentId) => AddUniversity(modelName),
                [1] = AddFaculty,
                [2] = AddDiscipline,
                [3] = AddCourse
            };
        }

        private static void CreateDirectory(string path, string newFolder = null)
        {
            var dirPath = Path.Combine(StoragePath, path);

            if (!(newFolder is null))
                dirPath = Path.Combine(dirPath, newFolder);

            Directory.CreateDirectory(dirPath);
        }

        #region Get

        private static readonly Dictionary<int, Func<int, IEnumerable<IEntityDTO>>> GetCmd;

        public static List<IEntityDTO> GetModelsList(int depth, int id = -1)
        {
            var result = GetCmd[depth].Invoke(id).ToList();

            if (!result.Any())
                result.Add(new NotFoundDTO());

            return result;
        }

        private static IEnumerable<IEntityDTO> GetUniversities()
            => UniversityDataProvider.GetUniversities();
        private static IEnumerable<IEntityDTO> GetFaculties(int universityID)
            => FacultyDataProvider.GetFaculties(universityID);
        private static IEnumerable<IEntityDTO> GetDisciplines(int facultyID)
            => DisciplineDataProvider.GetDisciplines(facultyID);
        private static IEnumerable<IEntityDTO> GetCourses(int disciplineID)
            => CourseDataProvider.GetCourses(disciplineID);

        public static IEnumerable<IEntityDTO> GetFilesList(int courseID, string filePath)
            => FileDataProvider.GetFiles(new DirectoryInfo(Path.Combine(StoragePath, filePath)), courseID);
        
        #endregion

        #region Add

        private static readonly Dictionary<int, Func<string, int, IEntityDTO>> AddCmd;

        public static IEntityDTO AddNewModel(int depth, string modelName, int parentId)
        {
            return AddCmd[depth].Invoke(modelName, parentId);
        }

        private static IEntityDTO AddUniversity(string universityName)
            => UniversityDataProvider.AddUniversity(universityName);
        private static IEntityDTO AddFaculty(string facultyName, int universityID)
            => FacultyDataProvider.AddFaculty(facultyName, universityID);
        private static IEntityDTO AddDiscipline(string disciplineName, int facultyID)
            => DisciplineDataProvider.AddDiscipline(disciplineName, facultyID);
        private static IEntityDTO AddCourse(string teacherName, int disciplineID)
            => CourseDataProvider.AddCourse(teacherName, disciplineID);

        public static IEntityDTO UploadFile(byte[] file, string filePath, int courseID)
            => FileDataProvider.UploadFile(file, Path.Combine(StoragePath, filePath), courseID);

        #endregion

        public static IEnumerable<IEntityDTO> FindFiles(int depth, string searchQuery)
        {
            //var searchPath = GetDirectory(depth).ToString();

            //return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
            //    .AsParallel()
            //    .Select(filePath => (path: filePath, pageEntries: FileReader.PdfSearch(filePath, searchQuery)))
            //    .Where(file => file.pageEntries.Any())
            //    .Select(file => FileDataProvider.GetSearchResultDTO(new FileInfo(file.path), file.pageEntries, StoragePath))
            //    .AsEnumerable();

            return null;
        }

        public static byte[] ReadFile(string filePath)
        {
            return File.ReadAllBytes(Path.Combine(StoragePath, filePath));
        }
    }
}
