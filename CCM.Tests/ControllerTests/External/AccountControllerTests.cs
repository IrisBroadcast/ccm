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
