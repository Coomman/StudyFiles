using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string _storagePath = @"res\";

        #region Get

        public ObservableCollection<IEntityDTO> GetModelsList(int depth, Guid id)
        {
            return depth switch
            {
                0 => new ObservableCollection<IEntityDTO>(GetUniversities()),
                1 => new ObservableCollection<IEntityDTO>(GetFaculties(id)),
                2 => new ObservableCollection<IEntityDTO>(GetDisciplines(id)),
                3 => new ObservableCollection<IEntityDTO>(GetCourses(id)),
                _ => null
            };
        }

        private IEnumerable<UniversityDTO> GetUniversities()
        {
            return UniversityDataProviderMock.GetUniversities();
        }
        private IEnumerable<FacultyDTO> GetFaculties(Guid universityID)
        {
            _curUniversityID = universityID;
            return FacultyDataProviderMock.GetFaculties(_curUniversityID);
        }
        private IEnumerable<DisciplineDTO> GetDisciplines(Guid facultyID)
        {
            _curFacultyID = facultyID;
            return DisciplineDataProviderMock.GetDisciplines(_curFacultyID);
        }
        private IEnumerable<CourseDTO> GetCourses(Guid disciplineID)
        {
            _curDisciplineID = disciplineID;
            return CourseDataProviderMock.GetCourses(_curDisciplineID);
        }

        #endregion

        #region Add

        public IEntityDTO AddNewModel(int depth, string modelName)
        {
            return depth switch
            {
                0 => AddUniversity(modelName),
                1 => AddFaculty(modelName),
                2 => AddDiscipline(modelName),
                3 => AddCourse(modelName),
                _ => null
            };
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

        #endregion


        public void UploadFile(int courseID, string filePath)
        {
            var dir = Directory.CreateDirectory(
                $@"{_storagePath}\{_curUniversityID}\{_curFacultyID}\{_curDisciplineID}\{courseID}");

            var files = dir.GetFiles();

            if(files.Any())
                File.Copy(filePath, dir.FullName + $@"\{int.Parse(files.Last().Name) + 1}.txt");
            else
                File.Copy(filePath, dir.FullName + @"\1.txt");
        }
        public FileInfo[] ShowFiles(Guid courseID)
        {
            _curCourseID = courseID;

            var dir = Directory.CreateDirectory(
                $@"{_storagePath}\{_curUniversityID}\{_curFacultyID}\{_curDisciplineID}\{courseID}");

            return dir.GetFiles();
        }
        public string ReadFile(string fileName)
        {
            var dir = Directory.CreateDirectory(
                $@"{_storagePath}\{_curUniversityID}\{_curFacultyID}\{_curDisciplineID}\{_curCourseID}");

            var file = dir.GetFiles().FirstOrDefault(f => f.Name == fileName);

            return file == null 
                ? null 
                : File.ReadAllText(file.FullName);
        }

        public List<string> SearchFiles(int depth, string query)
        {
            var searchPath = _storagePath.Clone() as string;

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
