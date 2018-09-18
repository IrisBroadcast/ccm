using System.Collections.Generic;
using System.Web.Http;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Web.Controllers.Api
{
    public class OngoingCallController : ApiController
    {
        #region Constructor and members
        private readonly ICallRepository _callRepository;

        public OngoingCallController(ICallRepository callRepository)
        {
            _callRepository = callRepository;
        }
        #endregion

        public IList<OnGoingCall> Post()
        {
            return _callRepository.GetOngoingCalls(true);
        }
    }
}
