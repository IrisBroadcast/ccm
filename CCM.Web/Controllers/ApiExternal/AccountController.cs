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
using System.Web.Http;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Models.ApiExternal;
using CCM.WebCommon.Authentication;
using CCM.WebCommon.Infrastructure.WebApi;
using NLog;

namespace CCM.Web.Controllers.ApiExternal
{
    /// <summary>
    /// Creating accounts from external service.
    /// </summary>
    [CcmUserBasicAuthentication]
    [CcmApiAuthorize(Roles = "Admin,AccountManager")]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ISipAccountManager _sipAccountManager;
        private readonly IOwnersRepository _ownersRepository;
        private readonly ICodecTypeRepository _codecTypeRepository;

        public AccountController(ISipAccountManager userManager, IOwnersRepository ownersRepository, ICodecTypeRepository codecTypeRepository)
        {
            _sipAccountManager = userManager;
            _ownersRepository = ownersRepository;
            _codecTypeRepository = codecTypeRepository;
        }

        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(string username)
        {
            SipAccount user = _sipAccountManager.GetByUserName(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserModel()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Comment = user.Comment
            });
        }

        [ValidateModel]
        [HttpPost]
        [Route("add")]
        public IHttpActionResult Add(AddUserModel model)
        {
            log.Debug("Call to ExternalAccountController.AddUser");

            if (model == null)
            {
                return BadRequest("Parameters missing.");
            }

            var user = new SipAccount
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName.Trim(),
                DisplayName = model.DisplayName,
                Comment = model.Comment,
                Owner = _ownersRepository.GetByName(Owners.SrOwnerName),
                CodecType = _codecTypeRepository.Find(CodecTypes.Personliga).FirstOrDefault(),
                Password = model.Password
            };

            try
            {
                var existingUser = _sipAccountManager.GetByUserName(user.UserName);
                if (existingUser != null)
                {
                    return Conflict();
                }

                _sipAccountManager.Create(user);
                return Created(Url.Content("get?username=" + user.UserName), "User created");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Could not create user");
                return InternalServerError();
            }
        }

        [HttpPost]
        [ValidateModel]
        [Route("update")]
        public IHttpActionResult Update(UserModel model)
        {
            log.Debug("Call to ExternalAccountController.EditUser");

            if (model == null)
            {
                return BadRequest("Parameters missing.");
            }

            var user = _sipAccountManager.GetByUserName(model.UserName);

            if (user == null)
            {
                return NotFound();
            }

            // Sätt nya värden på uppdateringsbara egenskaper
            user.DisplayName = model.DisplayName;
            user.Comment = model.Comment;

            try
            {
                _sipAccountManager.Update(user);
                return Ok("User updated");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Could not update user");
                return InternalServerError(new ApplicationException("User could not be updated."));
            }

        }

        [HttpPost]
        [ValidateModel]
        [Route("updatepassword")]
        public IHttpActionResult UpdatePassword(ChangePasswordModel model)
        {
            log.Debug("Call to ExternalAccountController.UpdatePassword");

            if (model == null)
            {
                return BadRequest("Parameters missing.");
            }
            
            var user = _sipAccountManager.GetByUserName(model.UserName);

            if (user == null)
            {
                return NotFound();
            }
            
            try
            {
                _sipAccountManager.UpdatePassword(user.Id, model.NewPassword);
                return Ok("Password updated");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Could not update password");
                return InternalServerError(new ApplicationException("Password could not be updated."));
            }
        }

        [ValidateModel]
        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("User name missing");
            }

            SipAccount user = _sipAccountManager.GetByUserName(userName);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                if (!_sipAccountManager.Delete(user.Id))
                {
                    return InternalServerError(new ApplicationException("User not deleted"));
                }
                
                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                log.Error(ex, "Could not delete user");
                return InternalServerError(new ApplicationException("User could not be deleted. " + ex.Message));
            }
        }
       
    }
}
