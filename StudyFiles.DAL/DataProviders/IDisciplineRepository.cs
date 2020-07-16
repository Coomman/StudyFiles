using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public interface IDisciplineRepository
    {
        public IEnumerable<DisciplineDTO> GetDisciplines(int facultyID);

        public IEntityDTO AddDiscipline(string name, int facultyID);
    }
}
