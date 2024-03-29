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
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Infrastructure;
using CCM.Web.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class CodecTypesController : Controller
    {
        private readonly ICodecTypeRepository _codecTypeRepository;
        private readonly IStringLocalizer<Resources> _localizer;

        public CodecTypesController(
            ICodecTypeRepository codecTypeRepository,
            IStringLocalizer<Resources> localizer)
        {
            _codecTypeRepository = codecTypeRepository;
            _localizer = localizer;
        }

        public ActionResult Index(string search = "")
        {
            var codecTypes = string.IsNullOrWhiteSpace(search) ? _codecTypeRepository.GetAll(true) : _codecTypeRepository.Find(search, true);
            ViewData["SearchString"] = search;
            return View(codecTypes);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create(CodecType model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;

                _codecTypeRepository.Save(model);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Name", _localizer["Name_Required"]);
            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var codecType = _codecTypeRepository.GetById(id);

            if (codecType == null)
            {
                return RedirectToAction("Index");
            }

            return View(codecType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(CodecType model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.UpdatedBy = User.Identity.Name;
                _codecTypeRepository.Save(model);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Name", _localizer["Name_Required"]);
            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var codecType = _codecTypeRepository.GetById(id);
            if (codecType == null)
            {
                return RedirectToAction("Index");
            }

            return View(codecType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(CodecType model)
        {
            _codecTypeRepository.Delete(model.Id);
            return RedirectToAction("Index");
        }
    }
}
