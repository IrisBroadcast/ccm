using System.Collections.Generic;
using System.Web.Http;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Web.Controllers.Api
{
    public class OldCallFilteredController : ApiController
    {
        #region Constructor and members
        private readonly ICallHistoryRepository _callHistoryRepository;
        private readonly ISettingsManager _settingsManager;

        public OldCallFilteredController(ICallHistoryRepository callHistoryRepository, ISettingsManager settingsManager)
        {
            _callHistoryRepository = callHistoryRepository;
            _settingsManager = settingsManager;
        }
        #endregion

        public IList<OldCall> Get(string region = "", string codecType = "", string search = "")
        {
            var oldCalls = _callHistoryRepository.GetOldCallsFiltered(region, codecType, "", search, true, false, _settingsManager.LatestCallCount);
            return oldCalls;
        }
    }
}