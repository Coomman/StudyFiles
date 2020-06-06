using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using StudyFiles.DTO;
using StudyFiles.DAL.DataProviders;

namespace StudyFiles.Core
{
    public class Facade
    {
        private Guid _curUniversityID;
        private Guid _curFacultyID;
        private Guid _curDisciplineID;
        private Guid _curCourseID;

        private const string StoragePath = @"res\";

        public Facade()
        {
            _getCmd = new Dictionary<int, Func<Guid, IEnumerable<IEntityDTO>>>
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

        private readonly Dictionary<int, Func<Guid, IEnumerable<IEntityDTO>>> _getCmd;

        public ObservableCollection<IEntityDTO> GetModelsList(int depth, Guid id)
        {
            return new ObservableCollection<IEntityDTO>(_getCmd[depth].Invoke(id));
        }

        private IEnumerable<IEntityDTO> GetUniversities()
        {
            return UniversityDataProviderMock.GetUniversities();
        }
        private IEnumerable<IEntityDTO> GetFaculties(Guid universityID)
        {
            _curUniversityID = universityID;
            return FacultyDataProviderMock.GetFaculties(_curUniversityID);
        }
        private IEnumerable<IEntityDTO> GetDisciplines(Guid facultyID)
        {
            _curFacultyID = facultyID;
            return DisciplineDataProviderMock.GetDisciplines(_curFacultyID);
        }
        private IEnumerable<IEntityDTO> GetCourses(Guid disciplineID)
        {
            _curDisciplineID = disciplineID;
            return CourseDataProviderMock.GetCourses(_curDisciplineID);
        }

        private IEnumerable<IEntityDTO> GetFiles(Guid courseID)
        {
            _curCourseID = courseID;
            return FileDataProvider.GetFiles(GetDirectory());
        }

        #endregion

        #region Add

        private readonly Dictionary<int, Func<string, IEntityDTO>> _addCmd;

        public IEntityDTO AddNewModel(int depth, string modelName)
        {
            return _addCmd[depth].Invoke(modelName);
        }

        private IEntityDTO AddUniversity(string universityName)
        {
            var uni = new UniversityDTO(Guid.NewGuid(), universityName);

            UniversityDataProviderMock.AddUniversity(uni);

            return uni;
        }
        private IEntityDTO AddFaculty(string facultyName)
        {
            var faculty = new FacultyDTO(Guid.NewGuid(), facultyName, _curUniversityID);

            FacultyDataProviderMock.AddFaculty(faculty);

            return faculty;
        }
        private IEntityDTO AddDiscipline(string disciplineName)
        {
            var disc = new DisciplineDTO(Guid.NewGuid(), disciplineName, _curFacultyID);

            DisciplineDataProviderMock.AddDiscipline(disc);

            return disc;
        }
        private IEntityDTO AddCourse(string teacherName)
        {
            var course = new CourseDTO(Guid.NewGuid(), teacherName, _curDisciplineID);

            CourseDataProviderMock.AddCourse(course);

            return course;
        }

        private IEntityDTO UploadFile(string filePath)
        {
            return FileDataProvider.UploadFile(GetDirectory(), filePath);
        }

        #endregion

        public string ReadFile(string fileName)
        {
            var filePath = Path.Combine(GetDirectory().FullName, fileName);

            return FileReader.ReadFile(new FileInfo(filePath));
        }

        // TODO: 1) Fix file displaying +--
        // ?? 2) File conversion when adding
        // 3) Fix file view
        // 4) Switch to commands
        // 5) Add search
        // 6) Switch to DataGrid

        public List<string> SearchFiles(int depth, string query)
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

            return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                .Where(f => File.ReadAllText(f).Contains(query)).ToList();
        }
    }
}
