using System;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;

namespace CCM.Web.Controllers
{
    public class PresetsController : Controller
    {
        private readonly ICodecPresetRepository _codecPresetRepository;

        public PresetsController(ICodecPresetRepository codecPresetRepository)
        {
            _codecPresetRepository = codecPresetRepository;
        }
        
        public ActionResult Index(string search = "")
        {
            var codecPresets = string.IsNullOrWhiteSpace(search) ? _codecPresetRepository.GetAll() : _codecPresetRepository.Find(search);

            ViewBag.SearchString = search;
            return View(codecPresets);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create(CodecPreset model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;

                _codecPresetRepository.Save(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Name", Resources.Name_Required);

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            var codecPreset = _codecPresetRepository.GetById(id);

            if (codecPreset == null)
            {
                return RedirectToAction("Index");
            }

            return View(codecPreset);
        }

        [HttpPost]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(CodecPreset model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.UpdatedBy = User.Identity.Name;

                _codecPresetRepository.Save(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Name", Resources.Name_Required);

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            var codecPreset = _codecPresetRepository.GetById(id);

            if (codecPreset == null)
            {
                return RedirectToAction("Index");
            }

            return View(codecPreset);
        }

        [HttpPost]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(CodecPreset model)
        {
            _codecPresetRepository.Delete(model.Id);
            return RedirectToAction("Index");
        }
    }
}