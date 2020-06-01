using System;
using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class UniversityDataProviderMock
    {
        private static readonly List<UniversityDTO> Universities = new List<UniversityDTO>
        {
            new UniversityDTO (Guid.Parse("1bdc0e42-b2b4-4346-966b-2f72b28017d8"), "ITMO"),
            new UniversityDTO (Guid.Parse("1bd93581-927c-4eed-9566-57885f96399c"), "Polytech")
        };

        public static List<UniversityDTO> GetUniversities()
        {
            return Universities;
        }
        public static void AddUniversity(UniversityDTO university)
        {
            Universities.Add(university);
        }
    }
}
