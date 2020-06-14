using System;
using System.Collections.Generic;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class UniversityDataProviderMock
    {
        private static int NextID { get; set; } = 3;

        private static readonly List<UniversityDTO> Universities = new List<UniversityDTO>
        {
            new UniversityDTO (1, "ITMO"),
            new UniversityDTO (2, "Polytech")
        };

        public static List<UniversityDTO> GetUniversities()
        {
            return Universities;
        }
        public static IEntityDTO AddUniversity(string name)
        {
            var uni = new UniversityDTO(NextID++, name);

            Universities.Add(uni);

            return uni;
        }
    }
}
