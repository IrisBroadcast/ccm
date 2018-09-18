using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using CCM.Core.Managers;
using CCM.Data.Radius;
using CCM.Data.Repositories;
using CCM.Web.Controllers.ApiExternal;
using CCM.Web.Models.ApiExternal;
using FluentAssertions;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.ControllerTests.External
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new AccountController(
                new SipAccountManager(new SipAccountRepository(new CachingService()), new CachingService()),
                new OwnersRepository(new CachingService()),
                new CodecTypeRepository(new CachingService()));

            _sut.Url = new UrlHelper();
            _sut.Url.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/external/account/");
        }

        [Test]
        public void Should_get_sipAccount()
        {
            IHttpActionResult result = _sut.Get("andste01@acip.example.com");
            var okResult = result as OkNegotiatedContentResult<UserModel>;
            var user = okResult?.Content;
            Assert.IsNotNull(user);
            Assert.AreEqual("andste01@acip.example.com", user.UserName);
            Assert.AreEqual("Anders Stenberg", user.DisplayName);
            Assert.AreEqual(null, user.Comment);
        }

        [Test]
        public void Should_create_update_delete_account()
        {
            // Add
            var model = new AddUserModel
            {
                UserName = $"TestUser{DateTime.Now:yyyyMMddHHmmss}",
                DisplayName = "Test Testsson",
                Comment = "Kommentar",
                Password = "1234"
            };

            var addResult = _sut.Add(model) as CreatedNegotiatedContentResult<string>;
            Assert.AreEqual("User created", addResult?.Content);
            Assert.AreEqual($"http://localhost/api/external/account/get?username={model.UserName}", addResult?.Location.OriginalString);

            // Get
            var getResult = _sut.Get(model.UserName) as OkNegotiatedContentResult<UserModel>;
            var user2 = getResult?.Content;
            Assert.AreEqual(model.DisplayName, user2.DisplayName);
            Assert.AreEqual(model.Comment, user2.Comment);

            // Update
            model.DisplayName = "Update Updatesson";
            model.Comment = "New comment";
            var updateResult = _sut.Update(model) as OkNegotiatedContentResult<string>;
            Assert.AreEqual("User updated", updateResult?.Content);

            var changePasswordModel = new ChangePasswordModel() {UserName = model.UserName, NewPassword = "4321"};
            var changePwdResult = _sut.UpdatePassword(changePasswordModel) as OkNegotiatedContentResult<string>;
            Assert.AreEqual("Password updated", changePwdResult?.Content);

            // Get
            var getResult2 = _sut.Get(model.UserName) as OkNegotiatedContentResult<UserModel>;
            var user3 = getResult2?.Content;
            Assert.AreEqual(model.DisplayName, user3.DisplayName);
            Assert.AreEqual(model.Comment, user3.Comment);

            // Delete
            var deleteResult = _sut.Delete(model.UserName) as OkNegotiatedContentResult<string>;
            Assert.AreEqual("User deleted", deleteResult?.Content);
        }
        
    }
}