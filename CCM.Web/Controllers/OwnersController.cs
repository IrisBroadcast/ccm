using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class OwnersController : BaseController
    {
        private readonly IOwnersRepository ownersRepository;

        public OwnersController(IOwnersRepository ownersRepository)
        {
            this.ownersRepository = ownersRepository;
        }

        public ActionResult Index(string search = "")
        {
            List<Owner> owners = string.IsNullOrWhiteSpace(search) ? ownersRepository.GetAll() : ownersRepository.FindOwners(search);

            ViewBag.SearchString = search;
            return View(owners);
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
        public ActionResult Create(Owner model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;

                ownersRepository.Save(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Name", Resources.Name_Required);

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            Owner owner = ownersRepository.GetById(id);

            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Owner model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.UpdatedBy = User.Identity.Name;

                ownersRepository.Save(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Name", Resources.Name_Required);

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            Owner owner = ownersRepository.GetById(id);
            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Owner model)
        {
            ownersRepository.Delete(model.Id);
            return RedirectToAction("Index");
        }
    }
}