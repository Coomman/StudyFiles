using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using StudyFiles.DTO;
using StudyFiles.DAL.DataProviders;

namespace StudyFiles.Core
{
    //TODO
    // 1) Add: In file on search query entries fast scroll
    // 2) File conversion when adding
    // 3) Fix file preview displaying +--

    // 4*) Switch to DataGrid

    public class Facade
    {
        private readonly int[] _id = new int[4];

        private const string StoragePath = @"res\";

        public Facade()
        {
            _getCmd = new Dictionary<int, Func<int, IEnumerable<IEntityDTO>>>
            {
                [0] = id => GetUniversities(),
                [1] = GetFaculties,
                [2] = GetDisciplines,
                [3] = GetCourses,
                [4] = GetFiles
            };

            _addCmd = new Dictionary<int, Func<string, IEntityDTO>>
            {
                [0] = AddUniversity,
                [1] = AddFaculty,
                [2] = AddDiscipline,
                [3] = AddCourse,
                [4] = UploadFile
            };
        }

        private DirectoryInfo GetDirectory(int depth = 4, int newDir = -1)
        {
            var path = StoragePath;

            for (int i = 0; i < depth; i++)
                path = Path.Combine(path, _id[i].ToString());

            if (newDir != -1)
                path = Path.Combine(path, newDir.ToString());

            return Directory.CreateDirectory(path);
        }

        #region Get

        private readonly Dictionary<int, Func<int, IEnumerable<IEntityDTO>>> _getCmd;

        public List<IEntityDTO> GetModelsList(int depth, int id = -1)
        {
            var result = _getCmd[depth].Invoke(id).ToList();

            if(!result.Any())
                result.Add(new NotFoundDTO());

            return result;
        }

        private IEnumerable<IEntityDTO> GetUniversities()
        {
            return UniversityDataProvider.GetUniversities();
        }
        private IEnumerable<IEntityDTO> GetFaculties(int universityID)
        {
            _id[0] = universityID;
            return FacultyDataProvider.GetFaculties(universityID);
        }
        private IEnumerable<IEntityDTO> GetDisciplines(int facultyID)
        {
            _id[1] = facultyID;
            return DisciplineDataProvider.GetDisciplines(facultyID);
        }
        private IEnumerable<IEntityDTO> GetCourses(int disciplineID)
        {
            _id[2] = disciplineID;
            return CourseDataProvider.GetCourses(disciplineID);
        }
        private IEnumerable<IEntityDTO> GetFiles(int courseID)
        {
            _id[3] = courseID;
            return FileDataProvider.GetFiles(GetDirectory(), courseID);
        }

        #endregion

        #region Add

        private readonly Dictionary<int, Func<string, IEntityDTO>> _addCmd;

        public IEntityDTO AddNewModel(int depth, string modelName)
        {
            var newItem = _addCmd[depth].Invoke(modelName);
            GetDirectory(depth, newItem.ID);
            return newItem;
        }

        private IEntityDTO AddUniversity(string universityName)
            => UniversityDataProvider.AddUniversity(universityName);
        private IEntityDTO AddFaculty(string facultyName)
            => FacultyDataProvider.AddFaculty(facultyName, _id[0]);
        private IEntityDTO AddDiscipline(string disciplineName)
            => DisciplineDataProvider.AddDiscipline(disciplineName, _id[1]);
        private IEntityDTO AddCourse(string teacherName)
            => CourseDataProvider.AddCourse(teacherName, _id[2]);

        private IEntityDTO UploadFile(string filePath)
            => FileDataProvider.UploadFile(GetDirectory(), _id[3], filePath);

        #endregion

        public IEnumerable<IEntityDTO> FindFiles(int depth, string searchQuery)
        {
            var searchPath = GetDirectory(depth).ToString();

            return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                .AsParallel()
                .Select(filePath => (path: filePath, pageEntries: FileReader.PdfSearch(filePath, searchQuery)))
                .Where(file => file.pageEntries.Any())
                .Select(file => FileDataProvider.GetSearchResultDTO(new FileInfo(file.path), file.pageEntries, StoragePath))
                .AsEnumerable();
        }

        public FileViewDTO ReadFile(string filePath, string searchQuery = null, List<int> pageEntries = null)
        {
            if (searchQuery is null)
                filePath = Path.Combine(GetDirectory().FullName, filePath);

            return new FileViewDTO(filePath, FileReader.GetPdfImages(filePath, searchQuery, pageEntries));
        }
    }
}
