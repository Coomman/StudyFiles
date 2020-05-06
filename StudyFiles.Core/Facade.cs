using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudyFiles.DAL.DataProviders;
using StudyFiles.DTO;

namespace StudyFiles.Core
{
    public class Facade
    {
        private int _curUniversityID;
        private int _curFacultyID;
        private int _curDisciplineID;
        private int _curCourseID;

        private string _storagePath = @"res\";

        // TODO parsing ID from label-sender
        public List<UniversityDTO> GetUniversities()
        {
            return UniversityDataProviderMock.GetUniversities();
        }
        public List<FacultyDTO> GetFaculties(int universityID)
        {
            if (universityID != -1)
                _curUniversityID = universityID;

            return FacultyDataProviderMock.GetFaculties(_curUniversityID);
        }
        public List<DisciplineDTO> GetDisciplines(int facultyID)
        {
            if(facultyID != -1)
                _curFacultyID = facultyID;

            return DisciplineDataProviderMock.GetDisciplines(_curFacultyID);
        }
        public List<CourseDTO> GetCourses(int disciplineID)
        {
            if(disciplineID != -1)
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
                Teacher = teacherName,
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
        public FileInfo[] ShowFiles(int courseID)
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
    }
}
