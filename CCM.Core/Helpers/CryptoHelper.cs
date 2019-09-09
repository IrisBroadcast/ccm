/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
            // Returns string with a hash of 'password' + 'random salt'.

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
