using System.Collections.Generic;
using System.Web.Http;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Web.Controllers.Api
{
    public class OldCallController : ApiController
    {
        #region Constructor and members

        private readonly ICallHistoryRepository _callHistoryRepository;
        private readonly ISettingsManager _settingsManager;

        public OldCallController(ICallHistoryRepository callHistoryRepository, ISettingsManager settingsManager)
        {
            _callHistoryRepository = callHistoryRepository;
            _settingsManager = settingsManager;
        }
        #endregion

        public IList<OldCall> Post()
        {
            return _callHistoryRepository.GetOldCalls(_settingsManager.LatestCallCount, true);
        }
    }
}