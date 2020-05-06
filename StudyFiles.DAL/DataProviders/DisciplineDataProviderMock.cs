using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class DisciplineDataProviderMock
    {
        private static readonly List<DisciplineDTO> Disciplines = new List<DisciplineDTO>
        {
            new DisciplineDTO{ID = 1, Name = "Discrete Math", FacultyID = 1},
            new DisciplineDTO{ID = 3, Name = "Algorithms and Data Structures", FacultyID = 1},
            new DisciplineDTO{ID = 4, Name = "Linear Algebra", FacultyID = 1}
        };

        public static List<DisciplineDTO> GetDisciplines(int facultyID)
        {
            return Disciplines.Where(d => d.FacultyID == facultyID).ToList();
        }
        public static void AddDiscipline(DisciplineDTO discipline)
        {
            discipline.ID = Disciplines.Last().ID + 1;
            Disciplines.Add(discipline);
        }
    }
}
