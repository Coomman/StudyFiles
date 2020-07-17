using System.Collections.Generic;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Service;

namespace StudyFiles.DAL.Repositories.Catalog
{
    public interface IFacultyRepository
    {
        public IEnumerable<FacultyDTO> GetFaculties(int universityID);

        public IEntityDTO AddFaculty(string name, int universityID);

        public void DeleteFaculty(int id);
    }
}
