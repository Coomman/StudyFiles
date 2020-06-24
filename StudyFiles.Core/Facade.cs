using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using StudyFiles.DTO;
using StudyFiles.DAL.DataProviders;

namespace StudyFiles.Core
{
    // TODO: 2) Add FindFiles implementation (To ViewModel)
    // 3) Add Search Result View (both for file and files)
    // 4) File conversion when adding
    // 5) Fix file view
    // 6) Connect search with BL +-
    // 7) Fix file preview displaying +--

    // 8*) Switch to DataGrid

    public class Facade
    {
        private enum Entity { University, Faculty, Discipline, Course };

        private readonly int[] ID = new int[4];

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
            var path = Path.Combine(StoragePath, ID[0].ToString(), ID[1].ToString(), ID[2].ToString(), ID[3].ToString());

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
            ID[(int)Entity.University] = universityID;
            return FacultyDataProviderMock.GetFaculties(universityID);
        }
        private IEnumerable<IEntityDTO> GetDisciplines(int facultyID)
        {
            ID[(int)Entity.Faculty] = facultyID;
            return DisciplineDataProviderMock.GetDisciplines(facultyID);
        }
        private IEnumerable<IEntityDTO> GetCourses(int disciplineID)
        {
            ID[(int)Entity.Discipline] = disciplineID;
            return CourseDataProviderMock.GetCourses(disciplineID);
        }
        private IEnumerable<IEntityDTO> GetFiles(int courseID)
        {
            ID[(int)Entity.Course] = courseID;
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
            => FacultyDataProviderMock.AddFaculty(facultyName, ID[(int) Entity.University]);
        private IEntityDTO AddDiscipline(string disciplineName)
            => DisciplineDataProviderMock.AddDiscipline(disciplineName, ID[(int) Entity.Faculty]);
        private IEntityDTO AddCourse(string teacherName)
            => CourseDataProviderMock.AddCourse(teacherName, ID[(int) Entity.Discipline]);

        private IEntityDTO UploadFile(string filePath)
            => FileDataProvider.UploadFile(GetDirectory(), ID[(int) Entity.Course], filePath);

        #endregion

        public FileViewDTO ReadFile(string fileName)
        {
            var filePath = Path.Combine(GetDirectory().FullName, fileName);
            return new FileViewDTO(filePath);

            //return FileReader.ReadFile(new FileInfo(filePath));
        }

        public IEnumerable<IEntityDTO> FindFiles(int depth, string searchQuery)
        {
            var searchPath = StoragePath.Clone() as string;

            for (int i = 0; i < depth; i++)
                Path.Combine(searchPath, ID[i].ToString());

            return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                .AsParallel()
                .Where(filePath => FileReader.PdfSearch(filePath, searchQuery))
                .Select(filePath => FileDataProvider.GetSearchResultDTO(new FileInfo(filePath)))
                .AsEnumerable();
        }
    }
}
