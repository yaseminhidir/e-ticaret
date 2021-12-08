using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Core.Helpers
{

    public class SHA256Helper
    {
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public static HashWithSaltResult HashWithNewSalt(string password, int saltLength)
        {
            var salt = GenerateRandomCryptographicKey(saltLength);
            var digest = ComputeSha256Hash(password + salt);
            return new HashWithSaltResult(digest, salt);
        }

        public static HashWithSaltResult HashWithKnownSalt(string password, string salt)
        {
            var digest = ComputeSha256Hash(password + salt);
            return new HashWithSaltResult(digest, salt);
        }


        public static string GenerateRandomCryptographicKey(int keyLength)
        {
            return Convert.ToBase64String(GenerateRandomCryptographicBytes(keyLength));
        }

        public static byte[] GenerateRandomCryptographicBytes(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }
    }
    public class HashWithSaltResult
    {
        public string Salt { get; }
        public string Digest { get; set; }

        public HashWithSaltResult(string digest, string salt)
        {
            Salt = salt;
            Digest = digest;
        }

    }
}
