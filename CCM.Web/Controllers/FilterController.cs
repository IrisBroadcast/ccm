using System;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Models.Filter;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = Roles.Admin)]
    public class FilterController : BaseController
    {
        private readonly IFilterManager _filterManager;

        public FilterController(IFilterManager filterManager)
        {
            _filterManager = filterManager;
        }

        public ActionResult Index(string search = "")
        {
            var filters = string.IsNullOrWhiteSpace(search) ? _filterManager.GetAllFilters() : _filterManager.FindFilters(search);
            var model = filters.Select(filter => new FilterViewModel() {Id = filter.Id, Name = filter.Name}).ToList();

            ViewBag.SearchString = search;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new FilterFormViewModel
            {
                Filters = _filterManager.GetFilterProperties()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FilterFormViewModel model)
        {
            var availableFilters = _filterManager.GetFilterProperties();

            if (ModelState.IsValid)
            {
                if (_filterManager.CheckFilterNameAvailability(model.FilterName, model.Id))
                {
                    var availableFilter = availableFilters.FirstOrDefault(f => f.FilteringName == model.SelectedFilter);
                    if (availableFilter != null)
                    {
                        var filter = new Core.Entities.Filter()
                        {
                            FilteringName = availableFilter.FilteringName,
                            Name = model.FilterName,
                            TableName = availableFilter.TableName,
                            ColumnName = availableFilter.ColumnName,
                            CreatedBy = User.Identity.Name,
                            UpdatedBy = User.Identity.Name
                        };

                        _filterManager.Save(filter);

                        return RedirectToAction("Index");
                    }
                }
            }

            model.Filters = availableFilters;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var filter = _filterManager.GetFilter(id);
            if (filter == null)
            {
                return RedirectToAction("Index");
            }

            var model = new FilterFormViewModel();
            model.Filters = _filterManager.GetFilterProperties();
            model.FilterName = filter.Name;
            model.Id = filter.Id;
            model.SelectedFilter = filter.FilteringName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilterFormViewModel model)
        {
            var availableFilters = _filterManager.GetFilterProperties();
            if (ModelState.IsValid)
            {
                if (_filterManager.CheckFilterNameAvailability(model.FilterName, model.Id))
                {
                    var availableFilter = availableFilters.FirstOrDefault(f => f.FilteringName == model.SelectedFilter);
                    if (availableFilter != null)
                    {
                        var filter = new Core.Entities.Filter()
                        {
                            Id = model.Id,
                            FilteringName = availableFilter.FilteringName,
                            TableName = availableFilter.TableName,
                            Name = model.FilterName,
                            ColumnName = availableFilter.ColumnName,
                            UpdatedBy = User.Identity.Name
                        };

                        _filterManager.Save(filter);

                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError(string.Empty, Resources.Filter_Name_In_Use);
            }

            ModelState.AddModelError("NameMustBeUnique", "Filtret kunde inte sparas. Möjligen används filter-namnet redan.");
            
            model.Filters = availableFilters;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var filter = _filterManager.GetFilter(id);

            if (filter == null)
            {
                return RedirectToAction("Index");
            }

            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CCM.Core.Entities.Filter filter)
        {
            _filterManager.Delete(filter.Id);

            return RedirectToAction("Index");
        }
    }
}