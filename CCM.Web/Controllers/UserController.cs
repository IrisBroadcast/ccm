using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using NLog;
using CCM.Web.Models.User;

namespace CCM.Web.Controllers
{

    [CcmAuthorize(Roles = Roles.Admin)]
    public class UserController : BaseController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IRoleRepository _roleRepository;
        private readonly ICcmUserRepository _userRepository;

        public UserController(ICcmUserRepository userRepository, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index(string search = "")
        {
            var model = new UserViewModel
            {
                Users = !string.IsNullOrWhiteSpace(search) ? _userRepository.FindUsers(search) : _userRepository.GetAll()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new UserFormViewModel();

            var userIsAdmin = User.IsInRole(Roles.Admin);
            model.Roles = GetRoles(userIsAdmin);

            ViewBag.Title = Resources.New_User;
            return View("CreateEdit", model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            CcmUser user = _userRepository.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var model = new UserFormViewModel
            {
                Comment = user.Comment,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                RoleId = user.RoleId,
            };

            var userIsAdmin = User.IsInRole(Roles.Admin);
            model.Roles = GetRoles(userIsAdmin);
            ViewBag.Title = Resources.Edit_User;
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserFormViewModel model)
        {
            bool newUser = model.Id == Guid.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new CcmUser
                    {
                        Id = model.Id,
                        UserName = model.UserName.Trim(),
                        FirstName = model.FirstName ?? string.Empty,
                        LastName = model.LastName ?? string.Empty,
                        Comment = model.Comment ?? string.Empty,
                        RoleId = model.RoleId,
                        Password = model.Password
                    };

                    if (newUser)
                    {
                        if (_userRepository.GetByUserName(user.UserName) != null)
                        {
                            log.Warn("Can't create user. Username {0} already exists in CCM database", user.UserName);
                            ModelState.AddModelError("SaveUser", Resources.User_Name_Already_Taken);
                        }
                        else
                        {
                            _userRepository.Create(user);
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        _userRepository.Update(user);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Could not save user");
                ModelState.AddModelError("SaveUser", Resources.User_could_not_be_saved);
            }

            var userIsAdmin = User.IsInRole(Roles.Admin);
            model.Roles = GetRoles(userIsAdmin);
            ViewBag.Title = newUser ? Resources.New_User : Resources.Edit_User;
            return View("CreateEdit", model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            CcmUser user = _userRepository.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var model = new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteUserViewModel model)
        {
            _userRepository.Delete(model.Id);
            return RedirectToAction("Index");
        }

        private List<CcmRole> GetRoles(bool includeAdminRole = true)
        {
            var allRoles = _roleRepository.GetRoles();
            var roles = includeAdminRole ? allRoles : allRoles.Where(r => r.Name != Roles.Admin).ToList();
            roles.Insert(0, new CcmRole() { Name = string.Empty, Id = Guid.Empty });
            return roles;

        }

    }
}