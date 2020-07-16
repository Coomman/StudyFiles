using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public interface IFacultyRepository
    {
        public IEnumerable<FacultyDTO> GetFaculties(int universityID);

        public IEntityDTO AddFaculty(string name, int universityID);
    }
}
