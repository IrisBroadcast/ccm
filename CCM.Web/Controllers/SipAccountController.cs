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
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Enums;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Extensions;
using CCM.Web.Infrastructure;
using CCM.Web.Models.SipAccount;
using CCM.Web.Models.User;
using NLog;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = Roles.Admin)]
    public class SipAccountController : BaseController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISipAccountManager _sipAccountManager;
        private readonly ICodecTypeRepository _codecTypeRepository;
        private readonly IOwnersRepository _ownersRepository;

        public SipAccountController(ISipAccountManager userManager, ICodecTypeRepository codecTypeRepository, IOwnersRepository ownersRepository)
        {
            _sipAccountManager = userManager;
            _codecTypeRepository = codecTypeRepository;
            _ownersRepository = ownersRepository;
        }

        public ActionResult Index(string search = "")
        {
            var sipAccounts = string.IsNullOrWhiteSpace(search)
                ? _sipAccountManager.GetAll()
                : _sipAccountManager.Find(search);

            var defaultCodecType = new CodecType() { Id = Guid.NewGuid(), Name = "Oklassificerade" };
            sipAccounts.ForEach(a => { a.CodecType = a.CodecType ?? defaultCodecType; });

            sipAccounts =sipAccounts.OrderBy(a => a.CodecType.Name ?? string.Empty).ThenBy(a => a.UserName ?? string.Empty).ToList();

            ViewBag.search = search;
            return View(new SipAccountViewModel { Users = sipAccounts });
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new SipAccountFormViewModel();
            SetListData(model);
            ViewBag.Title = Resources.New_Account;
            return View("CreateEdit", model);
        }
        
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            SipAccount user = _sipAccountManager.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var model = new SipAccountFormViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Comment = user.Comment,
                ExtensionNumber = user.ExtensionNumber,
                AccountLocked = user.AccountLocked,
                AccountType = user.AccountType,
                OwnerId = user.Owner?.Id ?? Guid.Empty,
                CodecTypeId = user.CodecType?.Id ?? Guid.Empty,
            };

            SetListData(model);
            ViewBag.Title = Resources.Edit_Account;
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(SipAccountFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new SipAccount
                {
                    Id = model.Id,
                    UserName = model.UserName.Trim(),
                    DisplayName = model.DisplayName,
                    Comment = model.Comment,
                    ExtensionNumber = model.ExtensionNumber,
                    AccountType = model.AccountType,
                    AccountLocked = model.AccountLocked,
                    Password = model.Password,
                    Owner = _ownersRepository.GetById(model.OwnerId),
                    CodecType = _codecTypeRepository.GetById(model.CodecTypeId),
                };
                
                try
                {
                    if (user.Id == Guid.Empty)
                    {
                        // New account
                        user.Id = Guid.NewGuid();
                        _sipAccountManager.Create(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Updated account
                        if (!string.IsNullOrWhiteSpace(model.Password))
                        {
                            _sipAccountManager.UpdatePassword(user.Id, model.Password);
                        }

                        _sipAccountManager.Update(user);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Could not create or edit sip account");
                    if (ex is ApplicationException)
                    {
                        ModelState.AddModelError("CreateUser", ex.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("CreateUser", "Användaren kunde inte sparas");
                    }
                    
                }
            }

            SetListData(model);
            return View("CreateEdit", model);
        }


        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            SipAccount user = _sipAccountManager.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var model = new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            ViewBag.Title = Resources.New_User;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteSipAccountViewModel model)
        {
            _sipAccountManager.Delete(Guid.Parse(model.Id));
            return RedirectToAction("Index");
        }
        
        private void SetListData(SipAccountFormViewModel model)
        {
            model.Owners = _ownersRepository.GetAll();
            model.Owners.Insert(0, new Owner { Id = Guid.Empty, Name = string.Empty });

            model.CodecTypes = _codecTypeRepository.GetAll(false);
            model.CodecTypes.Insert(0, new CodecType { Id = Guid.Empty, Name = string.Empty });

            model.AccountTypes = EnumHelpers.EnumSelectList<SipAccountType>().OrderBy(e => e.Text).ToList();
        }
        
    }
}
