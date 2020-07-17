using System.Collections.Generic;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Catalog
{
    public interface IDisciplineRepository
    {
        public IEnumerable<DisciplineDTO> GetDisciplines(int facultyID);

        public IEntityDTO AddDiscipline(string name, int facultyID);

        public void DeleteDiscipline(int id);
    }
}
