using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Models.Profile;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class ProfileController : BaseController
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public ActionResult Index(string search = "")
        {
            ViewBag.SearchString = search;

            var profiles = string.IsNullOrWhiteSpace(search) ?
                _profileRepository.GetAll() :
                _profileRepository.FindProfiles(search);

            return View(profiles);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create()
        {
            ViewBag.Title = Resources.New_Profile;
            return View("CreateEdit", new ProfileViewModel());
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            Profile profile = _profileRepository.GetById(id);
            if (profile == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProfileViewModel
            {
                Description = profile.Description,
                Id = profile.Id,
                Name = profile.Name,
                Sdp = profile.Sdp,
            };

            ViewBag.Title = Resources.Edit_Profile;
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult CreateEdit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = new Profile()
                {
                    Description = model.Description,
                    Id = model.Id,
                    Name = model.Name,
                    Sdp = model.Sdp,
                    UpdatedBy = User.Identity.Name,
                };

                if (profile.Id == Guid.Empty)
                {
                    profile.CreatedBy = User.Identity.Name;
                }

                try
                {
                    _profileRepository.Save(profile);
                }
                catch (DuplicateNameException)
                {
                    ModelState.AddModelError("NameMustBeUnique", "Profilen kunde inte sparas. Namnet används redan.");
                    return View("CreateEdit", model);
                }
                return RedirectToAction("Index");
            }

            return View("CreateEdit", model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            Profile profile = _profileRepository.GetById(id);
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Profile profile)
        {
            _profileRepository.Delete(profile.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [CcmAuthorize(Roles = Roles.Admin)]
        public JsonResult SetProfileSortIndex(List<GuidSortIndexTuple> profileSortIndex)
        {
            var model = new SetSortIndexResultViewModel();

            if (profileSortIndex == null || profileSortIndex.Any(d => d.Id == Guid.Empty))
            {
                model.IndexSet = false;
            }
            else
            {
                var paramdata = profileSortIndex.Select(i => new Tuple<Guid, int>(i.Id, i.SortIndex)).ToList();
                _profileRepository.SetProfileSortIndex(paramdata);
            }
            return Json(model);
        }
    }
}