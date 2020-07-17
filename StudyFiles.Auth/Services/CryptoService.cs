using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace StudyFiles.Auth
{
    public sealed class CryptoService
    {
        public static byte[] GenerateSalt(string login)
        {
            using var crypto = SHA256.Create();

            return crypto.ComputeHash(BitConverter.GetBytes(DateTime.UtcNow.ToBinary())
                .Concat(Encoding.UTF8.GetBytes(login)).ToArray());
        }

        public static byte[] ComputeHash(string password, byte[] salt)
        {
            byte[] passHash;
            using (var crypto256 = SHA256.Create())
                passHash = crypto256.ComputeHash(Encoding.UTF8.GetBytes(password));

            var hash = new byte[passHash.Length + salt.Length];
            for (int i = 0; i < passHash.Length; i++)
            {
                hash[i] = i % 2 == 0 ? passHash[i / 2] : salt[i / 2];
            }
            for (int i = passHash.Length; i < hash.Length; i++)
            {
                hash[i] = i % 2 == 0 ? salt[i / 2] : passHash[i / 2];
            }

            using var crypto512 = SHA512.Create();
            return crypto512.ComputeHash(hash);
        }

        public static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}
