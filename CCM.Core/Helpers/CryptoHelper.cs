using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CCM.Core.Helpers
{
    public class CryptoHelper
    {
        //public static string Md5HashPassword(string password)
        //{
        //    var md5 = MD5.Create();
        //    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        //    return Convert.ToBase64String(hashBytes);
        //}

        public static string Md5HashSaltedPassword(string password)
        {
            // Returnerar en sträng med hashen av lösenord + slumpmässigt salt. Strängen innehåller hash + saltet.

            var rng = new RNGCryptoServiceProvider();

            byte[] saltArray = new Byte[20];
            rng.GetNonZeroBytes(saltArray);

            var md5 = MD5.Create();
            var passwordArray = Encoding.UTF8.GetBytes(password);
            var passwordArrayWithSalt = passwordArray.Concat(saltArray).ToArray();
            var hashBytes = md5.ComputeHash(passwordArrayWithSalt);
            var hashBytesWithSalt = hashBytes.Concat(saltArray).ToArray();
            return Convert.ToBase64String(hashBytesWithSalt);
        }

        public static string Md5HashSaltedPassword(string password, string salt)
        {
            var saltArray = Convert.FromBase64String(salt);
            return Md5HashSaltedPassword(password, saltArray);
        }

        public static string Md5HashSaltedPassword(string password, byte[] saltArray)
        {
            var md5 = MD5.Create();
            var passwordArray = Encoding.UTF8.GetBytes(password);
            var passwordArrayWithSalt = passwordArray.Concat(saltArray).ToArray();
            var hashBytes = md5.ComputeHash(passwordArrayWithSalt);
            return Convert.ToBase64String(hashBytes.ToArray());
        }

        //public static string GenerateSalt()
        //{
        //    var salt = GenerateSaltBytes();
        //    return Encoding.UTF8.GetString(salt);
        //}

        public static byte[] GenerateSaltBytes()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new Byte[20];
            rng.GetNonZeroBytes(salt);
            return salt;
        }
    }
}