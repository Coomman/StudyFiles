using System;
using System.Collections.Generic;
using System.Linq;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class DisciplineDataProviderMock
    {
        private static int NextID { get; set; } = 4;

        private static readonly List<DisciplineDTO> Disciplines = new List<DisciplineDTO>
        {
            new DisciplineDTO(1, "Discrete Math", 1),
            new DisciplineDTO(2, "Algorithms and Data Structures", 1),
            new DisciplineDTO (3, "Linear Algebra", 1)
        };

        public static List<DisciplineDTO> GetDisciplines(int facultyID)
        {
            return Disciplines.Where(d => d.FacultyID == facultyID).ToList();
        }
        public static IEntityDTO AddDiscipline(string name, int facultyID)
        {
            var discipline = new DisciplineDTO(NextID++, name, facultyID);

            Disciplines.Add(discipline);

            return discipline;
        }
    }
}
