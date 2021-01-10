using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace MeAgendaAi.Cryptography.Cryptography
{
    public class Encrypt
    {
        private const int HasingIterationsCount = 10000;

        public static bool CompareComputeHash(string password, string salt, string storedPassword)
        {
            string hashPassword = EncryptString(password, salt);

            return hashPassword.Equals(storedPassword);
        }

        public static string EncryptString(string str, string salt)
        {
            try
            {
                byte[] byteSalt = GenerateSalt(salt);

                byte[] bytes = ComputeHash(str, byteSalt);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString());
                }
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
             password: password,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA256,
             iterationCount: iterations,
             numBytesRequested: (256 / 8));
        }
    }
}
