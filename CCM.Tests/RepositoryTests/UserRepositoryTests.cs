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
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Repositories;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ICcmUserRepository _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new CcmUserRepository( new CachingService() );
        }

        [Test]
        public void GetAll()
        {
            var users = _sut.GetAll();
            Assert.IsNotNull(users);
            users.ForEach(Console.WriteLine);
        }


        [Test]
        public void Crud()
        {
            var user = new CcmUser
            {
                UserName = $"User{DateTime.Now:yyyyMMddHHmmss}",
                FirstName = "Förnamn",
                LastName = "Efternamn",
                Password = "Pepparkaka",
                Comment = "Testanvändare"
            };

            var createResult = _sut.Create(user);
            Assert.IsTrue(createResult);

            var user2 = _sut.GetById(user.Id);
            Assert.AreEqual("Förnamn", user2.FirstName);
            Assert.AreEqual("Efternamn", user2.LastName);
            Assert.AreEqual("Testanvändare", user2.Comment);
            Assert.AreEqual(user.UserName, user2.UserName);

            user2.FirstName = "Alvar";
            user2.LastName = "Dysterkvist";

            var updateResult = _sut.Update(user2);
            Assert.IsTrue(updateResult);

            var user3 = _sut.GetById(user2.Id);
            Assert.AreEqual("Alvar", user3.FirstName);
            Assert.AreEqual("Dysterkvist", user3.LastName);

            var deleteResult = _sut.Delete(user3.Id);
            Assert.IsTrue(deleteResult);

            var user4 = _sut.GetById(user3.Id);
            Assert.IsNull(user4);
        }

        [Test]
        public void Authenticate_with_password()
        {
            var user = new CcmUser
            {
                UserName = $"User{DateTime.Now:yyyyMMddHHmmss}",
                FirstName = "Förnamn",
                LastName = "Efternamn",
                Password = "Pepparkaka",
                Comment = "Testanvändare"
            };

            var createResult = _sut.Create(user);
            Assert.IsTrue(createResult);

            Assert.IsFalse(_sut.AuthenticateAsync("", "Pepparkaka").Result);
            Assert.IsFalse(_sut.AuthenticateAsync(user.UserName, "Lingonkaka").Result);
            Assert.IsTrue(_sut.AuthenticateAsync(user.UserName, "Pepparkaka").Result);

            // Clean up
            _sut.Delete(user.Id);
        }


        [Test]
        public void Authentication_should_success_with_updated_password()
        {
            var user = new CcmUser
            {
                UserName = $"User{DateTime.Now:yyyyMMddHHmmss}",
                FirstName = "Förnamn",
                LastName = "Efternamn",
                Password = "Prinskorv",
                Comment = "Testanvändare"
            };

            var createResult = _sut.Create(user);
            Assert.IsTrue(createResult);

            Assert.IsTrue(_sut.AuthenticateAsync(user.UserName, "Prinskorv").Result);

            user.Password = "Grynkorv";
            var updateResult = _sut.Update(user);
            Assert.IsTrue(updateResult);

            Assert.IsFalse(_sut.AuthenticateAsync(user.UserName, "Prinskorv").Result);
            Assert.IsTrue(_sut.AuthenticateAsync(user.UserName, "Grynkorv").Result);

            // Clean up
            _sut.Delete(user.Id);
        }
    }
}
