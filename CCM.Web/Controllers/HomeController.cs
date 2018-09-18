using System;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using CCM.Web.Authentication;
using CCM.Web.Infrastructure;
using CCM.Web.Infrastructure.SignalR;
using CCM.Web.Models.Home;

namespace CCM.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Constructor and members

        private readonly ICodecTypeRepository _codecTypeRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private readonly ISipAccountManager _sipAccountManager;
        private readonly IGuiHubUpdater _guiHubUpdater;
        private readonly IStatusHubUpdater _statusHubUpdater;

        public HomeController(IRegionRepository regionRepository, ICodecTypeRepository codecTypeRepository, IRegisteredSipRepository registeredSipRepository,
            ISipAccountManager sipAccountManager, IGuiHubUpdater guiHubUpdater, IStatusHubUpdater statusHubUpdater)
        {
            _regionRepository = regionRepository;
            _codecTypeRepository = codecTypeRepository;
            _registeredSipRepository = registeredSipRepository;
            _sipAccountManager = sipAccountManager;
            _guiHubUpdater = guiHubUpdater;
            _statusHubUpdater = statusHubUpdater;
        }
        #endregion

        public ActionResult Index()
        {
            var vm = new HomeViewModel
            {
                CodecTypes = _codecTypeRepository.GetAll(false).Select(ct => new CodecTypeViewModel { Name = ct.Name, Color = ct.Color }),
                Regions = _regionRepository.GetAllRegionNames()
            };

            return View(vm);
        }

        [CcmAuthorize(Roles = "Admin, Remote")]
        [HttpGet]
        public ActionResult EditRegisteredSipComment(Guid id)
        {
            var sip = _registeredSipRepository.GetCachedRegisteredSips().FirstOrDefault(rs => rs.Id == id);

            if (sip == null)
            {
                return null;
            }

            return PartialView("_SipCommentForm", new SipAccountComment { Comment = sip.Comment, SipAccountId = sip.Id });
        }

        [ValidateAntiForgeryToken]
        [CcmAuthorize(Roles = "Admin, Remote")]
        [HttpPost]
        public ActionResult EditRegisteredSipComment(SipAccountComment model)
        {
            if (model.SipAccountId != Guid.Empty)
            {
                _sipAccountManager.UpdateComment(model.SipAccountId, model.Comment);
            }

            var updateResult = new KamailioMessageHandlerResult()
            {
                ChangeStatus = KamailioMessageChangeStatus.CodecUpdated,
                ChangedObjectId = model.SipAccountId
            };

            _guiHubUpdater.Update(updateResult);
            _statusHubUpdater.Update(updateResult);

            return null;
        }

    }
}