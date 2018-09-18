using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Interfaces.Repositories.Specialized;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Models.Cities;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    public class CityController : BaseController
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILocationInfoRepository _locationInfoRepository;

        public CityController(ICityRepository cityRepository, ILocationInfoRepository locationInfoRepository)
        {
            _cityRepository = cityRepository;
            _locationInfoRepository = locationInfoRepository;
        }

        public ActionResult Index(string search = "")
        {
            List<City> cities = string.IsNullOrWhiteSpace(search) ? _cityRepository.GetAll() : _cityRepository.Find(search);

            var model = cities.Select(city => new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                Locations = city.Locations.Select(location => new LocationViewModel() { Id = location.Id, Name = location.Name }).ToList(),
            }).ToList();

            ViewBag.SearchString = search;
            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create()
        {
            var model = new CityViewModel
            {
                Locations = _locationInfoRepository.GetAll().Select(location => new LocationViewModel { Id = location.Id, Name = location.Name }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Create(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var city = ViewModelToCity(model);
                _cityRepository.Save(city);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(Guid id)
        {
            var city = _cityRepository.GetById(id);
            if (city == null)
            {
                return RedirectToAction("Index");
            }

            var model = CityToViewModel(city);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Edit(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var city = ViewModelToCity(model);
                _cityRepository.Save(city);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(Guid id)
        {
            var city = _cityRepository.GetById(id);
            if (city == null)
            {
                return RedirectToAction("Index");
            }

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = Roles.Admin)]
        public ActionResult Delete(City city)
        {
            _cityRepository.Delete(city.Id);
            return RedirectToAction("Index");
        }

        private CityViewModel CityToViewModel(City city)
        {
            var locationIds = city.Locations.Select(l => l.Id).ToList();

            var model = new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                Locations = _locationInfoRepository.GetAll().Select(location => new LocationViewModel
                {
                    Id = location.Id,
                    Name = location.Name,
                    Selected = locationIds.Contains(location.Id)
                }).ToList()
            };

            return model;
        }

        private City ViewModelToCity(CityViewModel model)
        {
            var city = new City
            {
                Id = model.Id,
                Name = model.Name,
                Locations = new List<Location>(),

                // TODO: Replace with this:
                //Locations = (model.Locations ?? new List<LocationViewModel>())
                //    .Where(l => l.Selected)
                //    .Select(l => new Location() {Id = l.Id, Name = l.Name})
                //    .ToList()

            };


            if (model.Locations != null)
            {
                foreach (var locationViewModel in model.Locations)
                {
                    if (locationViewModel.Selected)
                    {
                        city.Locations.Add(new Location()
                        {
                            Id = locationViewModel.Id,
                            Name = locationViewModel.Name
                        });
                    }
                }
            }

            return city;
        }
    }
}