using System;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Models.Meta;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = Roles.Admin)]
    public class MetaController : BaseController
    {
        private readonly IMetaRepository _metaRepository;

        public MetaController(IMetaRepository metaRepository)
        {
            _metaRepository = metaRepository;
        }

        public ActionResult Index(string search = "")
        {
            var metaTypes = string.IsNullOrWhiteSpace(search) ? _metaRepository.GetAll() : _metaRepository.FindMetaTypes(search);
            var model = metaTypes.Select(metaType => new MetaViewModel() {Id = metaType.Id, Name = metaType.Name}).ToList();

            ViewBag.SearchString = search;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new MetaFormViewModel {MetaTypeValues = _metaRepository.GetMetaTypeProperties()};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MetaFormViewModel model)
        {
            var availableMetaTypes = _metaRepository.GetMetaTypeProperties();
            if (ModelState.IsValid)
            {
                if (_metaRepository.CheckMetaTypeNameAvailability(model.MetaTypeName, model.Id))
                {
                    var availableMetaType = availableMetaTypes.FirstOrDefault(m => m.FullPropertyName == model.SelectedMetaTypeValue);
                    if (availableMetaType != null)
                    {
                        var metaType = new CCM.Core.Entities.MetaType()
                        {
                            FullPropertyName = availableMetaType.FullPropertyName,
                            Name = model.MetaTypeName,
                            PropertyName = availableMetaType.PropertyName,
                            Type = availableMetaType.Type,
                            CreatedBy = User.Identity.Name,
                            UpdatedBy = User.Identity.Name
                        };

                        _metaRepository.Save(metaType);

                        return RedirectToAction("Index");
                    }
                }
            }

            model.MetaTypeValues = availableMetaTypes;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var metaType = _metaRepository.GetById(id);
            if (metaType == null)
            {
                return RedirectToAction("Index");
            }

            var model = new MetaFormViewModel
            {
                MetaTypeValues = _metaRepository.GetMetaTypeProperties(),
                MetaTypeName = metaType.Name,
                Id = metaType.Id,
                SelectedMetaTypeValue = metaType.FullPropertyName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MetaFormViewModel model)
        {
            var availableMetaTypes = _metaRepository.GetMetaTypeProperties();
            if (ModelState.IsValid)
            {
                if (_metaRepository.CheckMetaTypeNameAvailability(model.MetaTypeName, model.Id))
                {
                    var availableMetaType = availableMetaTypes.FirstOrDefault(m => m.FullPropertyName == model.SelectedMetaTypeValue);
                    if (availableMetaType != null)
                    {
                        var metaType = new CCM.Core.Entities.MetaType()
                        {
                            Id = model.Id,
                            FullPropertyName = availableMetaType.FullPropertyName,
                            Name = model.MetaTypeName,
                            PropertyName = availableMetaType.PropertyName,
                            Type = availableMetaType.Type,
                            UpdatedBy = User.Identity.Name
                        };

                        _metaRepository.Save(metaType);

                        return RedirectToAction("Index");
                    }
                }
            }

            model.MetaTypeValues = availableMetaTypes;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var metaType = _metaRepository.GetById(id);

            if (metaType == null)
            {
                return RedirectToAction("Index");
            }

            return View(metaType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CCM.Core.Entities.MetaType metaType)
        {
            _metaRepository.Delete(metaType.Id);

            return RedirectToAction("Index");
        }
    }
}