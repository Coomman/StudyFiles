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
            const string query = "Select * from University";

            using var command = new SqlCommand(query);

            return DBHelper.GetData(new UniversityDTOMapper(), command);
        }
        public static IEntityDTO AddUniversity(string name)
        {
            const string query = "Insert into University ([Name]) " +
                                 "Output Inserted.ID " +
                                 "values (@name)";

            using var command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) {Value = name});

            var id = (int) DBHelper.ExecuteScalar(command);
            return new UniversityDTO(id, name);
        }
    }
}
