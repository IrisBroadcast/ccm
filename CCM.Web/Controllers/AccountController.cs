using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Models.Account;
using NLog;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CCM.Web.Controllers
{
    [CcmAuthorize]
    public class AccountController : Controller
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ICcmUserRepository _userRepository;
        
        public AccountController(ICcmUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _userRepository.AuthenticateAsync(model.UserName, model.Password))
                    {
                        var user = _userRepository.GetByUserName(model.UserName);

                        if (user != null)
                        {
                            SignIn(user, model.RememberMe);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    ModelState.AddModelError(string.Empty, Resources.Invalid_Username_Password);
                    }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, Resources.Invalid_Username_Password);
                }
            }

            return View(model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        private void SignIn(CcmUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim("FullName", $"{user.FirstName} {user.LastName}".Trim()));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? (ActionResult) Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
        
    }
}