using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MeAgendaAi.Cryptography.Cryptography
{
    public class Encrypt
    {
        private const int HasingIterationsCount = 10000;

        public static bool CompareComputeHash(string password, string salt, string storedPassword)
        {
            var hashPassword = EncryptString(password, salt);

            return hashPassword.Equals(storedPassword);
        }

        public static string EncryptString(string str, string salt)
        {
            try
            {
                var byteSalt = GenerateSalt(salt);

                var bytes = ComputeHash(str, byteSalt);

                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString());
                return builder.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static byte[] GenerateSalt(string salt)
        {
            return Encoding.ASCII.GetBytes(salt);
        }

        private static byte[] ComputeHash(string password, byte[] salt, int iterations = HasingIterationsCount)
        {
            return KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterations,
                256 / 8);
        }
    }
}