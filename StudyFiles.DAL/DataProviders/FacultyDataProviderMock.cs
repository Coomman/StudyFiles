using System;
using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class FacultyDataProviderMock
    {
        private static readonly List<FacultyDTO> Faculties = new List<FacultyDTO>
        {
            new FacultyDTO
            {
                ID = Guid.Parse("b4971e8a-6724-4650-b12f-c414a6f0e292"), Name = "FTMI",
                UniversityID = Guid.Parse("1bdc0e42-b2b4-4346-966b-2f72b28017d8")
            },
            new FacultyDTO
            {
                ID = Guid.Parse("214dec99-d49d-4503-bf54-62e324a0e509"), Name = "KTU",
                UniversityID = Guid.Parse("1bdc0e42-b2b4-4346-966b-2f72b28017d8")
            }
        };

        public static List<FacultyDTO> GetFaculties(Guid universityID)
        {
            return Faculties.Where(f => f.UniversityID == universityID).ToList();
        }
        public static void AddFaculty(FacultyDTO faculty)
        {
            faculty.ID = Guid.NewGuid();
            Faculties.Add(faculty);
        }
    }
}
