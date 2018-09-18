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