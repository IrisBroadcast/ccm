using System;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class CodecTypesController : BaseController
    {
        private readonly ICodecTypeRepository _codecTypeRepository;

        public CodecTypesController(ICodecTypeRepository codecTypeRepository)
        {
            _codecTypeRepository = codecTypeRepository;
        }

        public ActionResult Index(string search = "")
        {
            var codecTypes = string.IsNullOrWhiteSpace(search) ? _codecTypeRepository.GetAll(true) : _codecTypeRepository.Find(search, true);
            ViewBag.SearchString = search;
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

            ModelState.AddModelError("Name", Resources.Name_Required);
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

            ModelState.AddModelError("Name", Resources.Name_Required);
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