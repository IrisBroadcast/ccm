using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Models.Common;
using CCM.Web.Models.Studio;

namespace CCM.Web.Controllers
{
    // Studio administration
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class StudioController : Controller
    {

        private readonly IStudioRepository _studioRepository;

        public StudioController(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        public ActionResult Index(string search = "")
        {
            var studios = string.IsNullOrWhiteSpace(search) ? _studioRepository.GetAll() : _studioRepository.FindStudios(search);
            var model = studios.Select(StudioToViewModel).OrderBy(s => s.Name).ToList();
            ViewBag.SearchString = search;
            return View(model);
        }
        
        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create()
        {
            return View(new StudioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create(StudioViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var studio = ViewModelToStudio(model);
                    studio.CreatedBy = studio.UpdatedBy = User.Identity.Name;
                    studio.CreatedOn = studio.UpdatedOn = DateTime.UtcNow;
                    _studioRepository.Save(studio);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            var studio = _studioRepository.GetById(id);
            if (studio == null)
            {
                return RedirectToAction("Index");
            }

            var model = StudioToViewModel(studio);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(StudioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var studio = ViewModelToStudio(model);
                studio.UpdatedBy = User.Identity.Name;
                _studioRepository.Save(studio);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            var studio = _studioRepository.GetById(id);
            if (studio == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new DeleteViewModel()
            {
                Id = studio.Id,
                Name = studio.Name,
                Title = Resources.RemoveStudio,
                WarningText = Resources.RemoveStudioAreYouSure
            };

            return View("../Shared/Delete", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(DeleteViewModel model)
        {
            _studioRepository.Delete(model.Id);
            return RedirectToAction("Index");
        }

        #region Private methods
        private StudioViewModel StudioToViewModel(Studio studio)
        {
            return Mapper.Map<StudioViewModel>(studio);
        }

        private Studio ViewModelToStudio(StudioViewModel model)
        {
            return Mapper.Map<Studio>(model);
        }
        #endregion
    }
}
