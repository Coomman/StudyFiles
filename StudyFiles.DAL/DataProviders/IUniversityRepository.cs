using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public interface IUniversityRepository
    {
        public IEnumerable<UniversityDTO> GetUniversities();

        public IEntityDTO AddUniversity(string name);
    }
}
