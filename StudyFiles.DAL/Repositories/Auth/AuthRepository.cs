using System.Data;
using System.Data.SqlClient;
using StudyFiles.DAL.Mappers.Auth;
using StudyFiles.DTO.Auth;

namespace StudyFiles.DAL.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDBHelper _dbHelper;

        public AuthRepository(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void CreateAccount(string email, string login, byte[] passHash, byte[] salt)
        {
            using var command = new SqlCommand(Queries.CreateAccount);

            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@email", SqlDbType.VarChar) {Value = email}, 
                new SqlParameter("@login", SqlDbType.VarChar) {Value = login},
                new SqlParameter("@passHash", SqlDbType.VarBinary) {Value = passHash},
                new SqlParameter("@salt", SqlDbType.VarBinary) {Value = salt}
            });

            _dbHelper.ExecuteNonQuery(command);
        }

        public bool VerifyUserExistence(string login)
        {
            using var command = new SqlCommand(Queries.VerifyUserExistence);

            command.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar) { Value = login });

            return _dbHelper.ExecuteScalar<int?>(command) != null;
        }
        public AuthDTO GetUserData(string login)
        {
            using var command = new SqlCommand(Queries.GetUserData);

            command.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar) { Value = login });

            return _dbHelper.GetItem(new AuthDTOMapper(), command);
        }

        public void StartSession(int id, string token)
        {
            using var command = new SqlCommand(Queries.StartSession);

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = id});
            command.Parameters.Add(new SqlParameter("@token", SqlDbType.VarChar) {Value = token});

            _dbHelper.ExecuteNonQuery(command);
        }
        public bool ValidateToken(int id, string token)
        {
            using var command = new SqlCommand(Queries.ValidateToken);

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@token", SqlDbType.VarChar) { Value = token });

            return _dbHelper.ExecuteScalar<int?>(command) != null;
        }
        public LoginDTO RestoreSession(int id)
        {
            using var command = new SqlCommand(Queries.RestoreSession);

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            return _dbHelper.GetItem(new LoginDTOMapper(), command);
        }
        public void EndSession(int id)
        {
            using var command = new SqlCommand(Queries.EndSession);

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            _dbHelper.ExecuteNonQuery(command);
        }
    }
}
