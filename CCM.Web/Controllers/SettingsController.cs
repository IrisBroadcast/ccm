using System.Linq;
using System.Web.Mvc;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Managers;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Models;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = Roles.Admin)]
    public class SettingsController : BaseController
    {
        private readonly ISettingsManager _settingsManager;

        public SettingsController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public ActionResult Index()
        {
            var model = new SettingsViewModel { Settings = _settingsManager.GetSettings().OrderBy(s => s.Name).ToList() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SettingsViewModel model)
        {
            _settingsManager.SaveSettings(model.Settings, User.Identity.Name);
            return RedirectToAction("Index");
        }
    }
}