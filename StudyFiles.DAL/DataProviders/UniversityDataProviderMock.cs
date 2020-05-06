using System.Collections.Generic;
using System.Linq;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class UniversityDataProviderMock
    {
        private static readonly List<UniversityDTO> Universities = new List<UniversityDTO>
        {
            new UniversityDTO {ID = 1, Name = "ITMO"},
            new UniversityDTO {ID = 2, Name = "Polytech"}
        };

        public static List<UniversityDTO> GetUniversities()
        {
            return Universities;
        }
        public static void AddUniversity(UniversityDTO university)
        {
            university.ID = Universities.Last().ID + 1; //TODO binary search for the first empty ID
            Universities.Add(university);
        }
    }
}
