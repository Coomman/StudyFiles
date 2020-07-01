using System.Data.SqlClient;

namespace StudyFiles.DAL.Mappers
{
    internal class PathMapper : IMapper<(string path, string breadCrumb)>
    {
        public (string path, string breadCrumb) ReadItem(SqlDataReader dr)
        {
            return (
                (string) dr["Path"],
                (string) dr["BreadCrumb"]);
        }
    }
}
