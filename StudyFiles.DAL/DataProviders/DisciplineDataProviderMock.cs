using System;
using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class DisciplineDataProviderMock
    {
        private static readonly List<DisciplineDTO> Disciplines = new List<DisciplineDTO>
        {
            new DisciplineDTO(Guid.Parse("abecf0cc-ce2c-4667-9a2c-14bffe36bd7e"), "Discrete Math",
                Guid.Parse("b4971e8a-6724-4650-b12f-c414a6f0e292")),

            new DisciplineDTO(
            Guid.Parse("9489b3d7-dbfd-4d7d-b44a-02e7c93cd160"), "Algorithms and Data Structures",
                Guid.Parse("b4971e8a-6724-4650-b12f-c414a6f0e292")),

            new DisciplineDTO (Guid.Parse("7f700f22-eefc-4666-9964-e6470e8d9ab0"), "Linear Algebra",
                 Guid.Parse("b4971e8a-6724-4650-b12f-c414a6f0e292"))
        };

        public static List<DisciplineDTO> GetDisciplines(Guid facultyID)
        {
            return Disciplines.Where(d => d.FacultyID == facultyID).ToList();
        }
        public static void AddDiscipline(DisciplineDTO discipline)
        {
            Disciplines.Add(discipline);
        }
    }
}
