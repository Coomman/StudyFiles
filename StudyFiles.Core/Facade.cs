using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudyFiles.DTO;
using StudyFiles.DAL.DataProviders;

namespace StudyFiles.Core
{
    // TODO: 2) Add SearchFiles correct implementation
    // 3) Add Search Result View (both for file and files)
    // 4) File conversion when adding
    // 5) Fix file view
    // 6) Connect search with BL
    // 7) Fix file preview displaying +--

    // 8*) Switch to DataGrid

    public class Facade
    {
        private int _curUniversityID;
        private int _curFacultyID;
        private int _curDisciplineID;
        private int _curCourseID;

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

        private DirectoryInfo GetDirectory()
        {
            var path = Path.Combine(StoragePath,
                _curUniversityID.ToString(), _curFacultyID.ToString(), _curDisciplineID.ToString(), _curCourseID.ToString());

            return Directory.CreateDirectory(path);
        }

        #region Get

        private readonly Dictionary<int, Func<int, IEnumerable<IEntityDTO>>> _getCmd;

        public ObservableCollection<IEntityDTO> GetModelsList(int depth, int id = -1)
        {
            return new ObservableCollection<IEntityDTO>(_getCmd[depth].Invoke(id));
        }

        private IEnumerable<IEntityDTO> GetUniversities()
        {
            return UniversityDataProviderMock.GetUniversities();
        }
        private IEnumerable<IEntityDTO> GetFaculties(int universityID)
        {
            _curUniversityID = universityID;
            return FacultyDataProviderMock.GetFaculties(_curUniversityID);
        }
        private IEnumerable<IEntityDTO> GetDisciplines(int facultyID)
        {
            _curFacultyID = facultyID;
            return DisciplineDataProviderMock.GetDisciplines(_curFacultyID);
        }
        private IEnumerable<IEntityDTO> GetCourses(int disciplineID)
        {
            _curDisciplineID = disciplineID;
            return CourseDataProviderMock.GetCourses(_curDisciplineID);
        }

        private IEnumerable<IEntityDTO> GetFiles(int courseID)
        {
            _curCourseID = courseID;
            return FileDataProvider.GetFiles(GetDirectory(), courseID);
        }

        #endregion

        #region Add

        private readonly Dictionary<int, Func<string, IEntityDTO>> _addCmd;

        public IEntityDTO AddNewModel(int depth, string modelName)
        {
            return _addCmd[depth].Invoke(modelName);
        }

        private IEntityDTO AddUniversity(string universityName)
            => UniversityDataProviderMock.AddUniversity(universityName);
        private IEntityDTO AddFaculty(string facultyName)
            => FacultyDataProviderMock.AddFaculty(facultyName, _curUniversityID);
        private IEntityDTO AddDiscipline(string disciplineName)
            => DisciplineDataProviderMock.AddDiscipline(disciplineName, _curFacultyID);
        private IEntityDTO AddCourse(string teacherName)
            => CourseDataProviderMock.AddCourse(teacherName, _curDisciplineID);

        private IEntityDTO UploadFile(string filePath)
            => FileDataProvider.UploadFile(GetDirectory(), _curCourseID, filePath);

        #endregion

        public FileViewDTO ReadFile(string fileName)
        {
            var filePath = Path.Combine(GetDirectory().FullName, fileName);
            return new FileViewDTO(filePath);

            //return FileReader.ReadFile(new FileInfo(filePath));
        }

        public ObservableCollection<IEntityDTO> SearchFiles(int depth, string searchQuery)
        {
            var searchPath = StoragePath.Clone() as string;

            if (depth > 0)
                searchPath += $@"{_curUniversityID}";

            if (depth > 1)
                searchPath += $@"\{_curFacultyID}";

            if (depth > 2)
                searchPath += $@"\{_curDisciplineID}";

            if (depth > 3)
                searchPath += $@"\{_curCourseID}";

            return null;
            //return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
        }



        //public List<string> SearchFiles(int depth, string query)
        //{
        //    var searchPath = StoragePath.Clone() as string;

        //    if (depth > 0)
        //        searchPath += $@"{_curUniversityID}";

        //    if (depth > 1)
        //        searchPath += $@"\{_curFacultyID}";

        //    if (depth > 2)
        //        searchPath += $@"\{_curDisciplineID}";

        //    if (depth > 3)
        //        searchPath += $@"\{_curCourseID}";

        //    return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
        //        .Where(f => File.ReadAllText(f).Contains(query)).ToList();
        //}
    }
}
