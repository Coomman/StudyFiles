using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class FacultyDataProviderMock
    {
        private static readonly List<FacultyDTO> Faculties = new List<FacultyDTO>
        {
            new FacultyDTO {ID = 1, Name = "FTMI", UniversityID = 1},
            new FacultyDTO {ID = 2, Name = "KTU", UniversityID = 1}
        };

        public static List<FacultyDTO> GetFaculties(int universityID)
        {
            return Faculties.Where(f => f.UniversityID == universityID).ToList();
        }
        public static void AddFaculty(FacultyDTO faculty)
        {
            faculty.ID = Faculties.Last().ID + 1;
            Faculties.Add(faculty);
        }
    }
}
