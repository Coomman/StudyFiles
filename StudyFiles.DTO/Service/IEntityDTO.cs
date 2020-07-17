using JsonSubTypes;
using Newtonsoft.Json;
using StudyFiles.DTO.Catalog;
using StudyFiles.DTO.Files;

namespace StudyFiles.DTO.Service
{
    [JsonConverter(typeof(JsonSubtypes), "SubType")]
    [JsonSubtypes.KnownSubType(typeof(UniversityDTO), 0)]
    [JsonSubtypes.KnownSubType(typeof(FacultyDTO), 1)]
    [JsonSubtypes.KnownSubType(typeof(DisciplineDTO), 2)]
    [JsonSubtypes.KnownSubType(typeof(CourseDTO), 3)]
    [JsonSubtypes.KnownSubType(typeof(FileDTO), 4)]
    [JsonSubtypes.KnownSubType(typeof(FileViewDTO), 5)]
    [JsonSubtypes.KnownSubType(typeof(NotFoundDTO), 6)]
    [JsonSubtypes.KnownSubType(typeof(NullDTO), 7)]
    [JsonSubtypes.KnownSubType(typeof(SearchResultDTO), 8)]
    public interface IEntityDTO
    {
        int ID { get; set; }
        string InnerText { get; set; }

        int SubType { get; }
    }
}
