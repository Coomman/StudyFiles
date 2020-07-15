using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using StudyFiles.DAL.Mappers;
using StudyFiles.DTO;

namespace StudyFiles.DAL.DataProviders
{
    public static class UniversityDataProvider
    {
        public static List<UniversityDTO> GetUniversities()
        {
            using var command = new SqlCommand(Queries.GetUniversities);

            return DBHelper.GetData(new UniversityDTOMapper(), command);
        }
        public static IEntityDTO AddUniversity(string name)
        {
            using var command = new SqlCommand(Queries.AddUniversity);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) {Value = name});

            var id = (int) DBHelper.ExecuteScalar(command);
            return new UniversityDTO {ID = id, InnerText = name};
        }
    }
}
