using System.Collections.Generic;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Catalog
{
    public interface IUniversityRepository
    {
        public IEnumerable<UniversityDTO> GetUniversities();

        public IEntityDTO AddUniversity(string name);

        public void DeleteUniversity(int id);
    }
}
