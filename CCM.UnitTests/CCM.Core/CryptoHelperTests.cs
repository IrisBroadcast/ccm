using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CCM.Core.Helpers;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core
{
    [TestFixture]
    public class CryptoHelperTests
    {

        [Test]
        public void CheckPassword()
        {
            var hash = "yeWiadH0QT6kc/tz6QQjtJF9aBGXATnUXJNxnwa2HeV6B4Ti";
            byte[] bytes = Convert.FromBase64String(hash);
            Console.WriteLine("Hash:    " + AsHexString(bytes));

            var pwdHash = bytes.Take(16).ToArray();
            Console.WriteLine("PwdHash: " + AsHexString(pwdHash));

            var saltBytes = bytes.Skip(16).ToArray();
            Console.WriteLine("Salt:    " + AsHexString(saltBytes));
            var salt64 = Convert.ToBase64String(saltBytes);
            Console.WriteLine("Salt64:  " + salt64);

            var md5 = MD5.Create();
            var password = "helpdesk";
            var passwordArray = Encoding.UTF8.GetBytes(password);
            var passwordArrayWithSalt = passwordArray.Concat(saltBytes).ToArray();
            var hashBytes = md5.ComputeHash(passwordArrayWithSalt);
            var hashBytesWithSalt = hashBytes.Concat(saltBytes).ToArray();
            var hash2 = Convert.ToBase64String(hashBytesWithSalt);
            Console.WriteLine(hash2);

            Assert.AreEqual(hash, hash2);
        }

        [Test]
        public void DecryptBase64String()
        {
            var password = "test";
            var s = CryptoHelper.Md5HashSaltedPassword(password);

            //var s = "QUVgNJAwPAIg2t+p/oK8XlzMCtuCpyHiY/+GWR4UGlJA1KPt";
            //var s = "Fuydi2bLFQq09riETC+pqUcbApUaRI/UvRbN5cYX6yFYXfnm";
            //var s = "hvdfJktj2Dcqmh5e1gcmTjXoD9IHGx1meHMTeE/N6gy8qw+8";

            byte[] bytes = Convert.FromBase64String(s);


            //StringBuilder hex = new StringBuilder();
            //foreach (byte b in bytes)
            //{
            //    hex.AppendFormat("{0:x2}", b);
            //}

            //var s2 = hex.ToString();

            Console.WriteLine(AsHexString(bytes));
            Console.WriteLine(@"Length = " + bytes.Length);

            var pwdHash = bytes.Take(16).ToArray();
            var salt = bytes.Skip(16).ToArray();

            Console.WriteLine(AsHexString(pwdHash));
            Console.WriteLine(AsHexString(salt));

        }

        private static string AsHexString(byte[] bytes)
        {
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }
    }
}