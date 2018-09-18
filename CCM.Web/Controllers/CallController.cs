using System;
using System.Web.Mvc;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using CCM.Web.Models.Call;

namespace CCM.Web.Controllers
{
    public class CallController : Controller
    {
        private readonly ICallRepository _callRepository;

        public CallController(ICallRepository callRepository)
        {
            _callRepository = callRepository;
        }

        [CcmAuthorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCall(DeleteCallViewModel model)
        {
            if (model.CallId != Guid.Empty && model.IHaveChecked && model.ImSure)
            {
                _callRepository.CloseCall(model.CallId);
            }

            return RedirectToAction("Index", "Home");
        }


        [CcmAuthorize(Roles = Roles.Admin)]
        [HttpGet]
        public ActionResult DeleteCall(string id)
        {
            Guid callIdGuid;

            if (!Guid.TryParse(id, out callIdGuid))
            {
                return RedirectToAction("Index", "Home");
            }

            var call = _callRepository.GetCallInfoById(callIdGuid);

            if (call == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new DeleteCallViewModel()
            {
                CallId = callIdGuid,
                CallFromSipAddress = call.FromSipAddress,
                CallToSipAddress = call.ToSipAddress,
                CallStarted = call.Started.ToLocalTime()
            };

            return View(model);
        }
    }
}