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
        private static string ByteSizeToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB"};

            if (byteCount == 0)
                return "0 " + suf[0];

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return$"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
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

            var dir = GetDirectory();

            return dir.GetFiles()
                .Select(file => new FileDTO(Guid.Empty, file.Name, ByteSizeToString(file.Length), courseID, 
                    file.CreationTime.ToLongDateString()));
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
            var dir = GetDirectory();

            var files = dir.GetFiles();

            var fileName = files.Any()
                ? $@"\{int.Parse(files.Last().Name) + 1}.txt"
                : @"\1.txt";

            File.Copy(filePath, dir.FullName + fileName);

            var fileInfo = new FileInfo(filePath);
            
            return new FileDTO(Guid.Empty, fileInfo.Name, ByteSizeToString(fileInfo.Length), 
                _curCourseID, fileInfo.CreationTime.ToLongDateString());
        }

        #endregion

        public string ReadFile(string fileName)
        {
            var dir = Directory.CreateDirectory(
                $@"{StoragePath}\{_curUniversityID}\{_curFacultyID}\{_curDisciplineID}\{_curCourseID}");

            var file = dir.GetFiles().FirstOrDefault(f => f.Name == fileName);

            return file == null 
                ? null 
                : File.ReadAllText(file.FullName);
        }
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
