using System.Collections.Generic;
using System.Web.Http;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Web.Controllers.ApiExternal
{
    public class OldCallExternalController : ApiController
    {
        private readonly ICallHistoryRepository _callHistoryRepository;

        public OldCallExternalController(ICallHistoryRepository callHistoryRepository)
        {
            _callHistoryRepository = callHistoryRepository;
        }

        [Route("api/external/oldcall")]
        public IList<OldCall> Get(string region = "", string codecType = "", string sipAddress = "", string search = "", bool onlyPhoneCalls = false, int callCount = 20)
        {
            var oldCalls = _callHistoryRepository.GetOldCallsFiltered(region, codecType, sipAddress, search, false, onlyPhoneCalls, callCount);
            return oldCalls;
        }
    }
}