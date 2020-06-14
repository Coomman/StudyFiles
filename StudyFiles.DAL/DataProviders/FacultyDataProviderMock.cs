using System.Linq;
using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class FacultyDataProviderMock
    {
        private static int NextID { get; set; } = 3;

        private static readonly List<FacultyDTO> Faculties = new List<FacultyDTO>
        {
            new FacultyDTO(1, "FTMI", 1),
            new FacultyDTO (2, "KTU", 1)
        };

        public static List<FacultyDTO> GetFaculties(int universityID)
        {
            return Faculties.Where(f => f.UniversityID == universityID).ToList();
        }
        public static IEntityDTO AddFaculty(string name, int universityID)
        {
            var faculty = new FacultyDTO(NextID++, name, universityID);

            Faculties.Add(faculty);

            return faculty;
        }
    }
}
