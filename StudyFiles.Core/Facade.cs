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

        public List<UniversityDTO> GetUniversities()
        {
            return UniversityDataProviderMock.GetUniversities();
        }
        public List<FacultyDTO> GetFaculties(Guid universityID)
        {
            _curUniversityID = universityID;
            return FacultyDataProviderMock.GetFaculties(_curUniversityID);
        }
        public List<DisciplineDTO> GetDisciplines(Guid facultyID)
        {
            _curFacultyID = facultyID;
            return DisciplineDataProviderMock.GetDisciplines(_curFacultyID);
        }
        public List<CourseDTO> GetCourses(Guid disciplineID)
        {
            _curDisciplineID = disciplineID;
            return CourseDataProviderMock.GetCourses(_curDisciplineID);
        }

        // TODO receiving new object info from UI form and form updating
        public void AddUniversity(string universityName)
        {
            UniversityDataProviderMock.AddUniversity(new UniversityDTO
            {
                Name = universityName
            });
        }
        public void AddFaculty(string facultyName)
        {
            FacultyDataProviderMock.AddFaculty(new FacultyDTO
            {
                Name = facultyName,
                UniversityID = _curUniversityID
            });
        }
        public void AddDiscipline(string disciplineName)
        {
            DisciplineDataProviderMock.AddDiscipline(new DisciplineDTO
            {
                Name = disciplineName,
                FacultyID = _curFacultyID
            });
        }
        public void AddCourse(string teacherName)
        {
            CourseDataProviderMock.AddCourse(new CourseDTO
            {
                Name = teacherName,
                DisciplineID = _curDisciplineID
            });
        }

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
