using System;
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
            new UniversityDTO {ID = Guid.Parse("1bdc0e42-b2b4-4346-966b-2f72b28017d8"), Name = "ITMO"},
            new UniversityDTO {ID = Guid.Parse("1bd93581-927c-4eed-9566-57885f96399c"), Name = "Polytech"}
        };

        public static List<UniversityDTO> GetUniversities()
        {
            return Universities;
        }
        public static void AddUniversity(UniversityDTO university)
        {
            university.ID = Guid.NewGuid();
            Universities.Add(university);
        }
    }
}
