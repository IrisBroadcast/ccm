using System.Collections.Generic;
using CCM.Core.Entities.Specific;

namespace CCM.Web.Models.Home
{
    public class OngoingCallsUpdateViewModel
    {
        public int Count { get; set; }
        public IList<OnGoingCall> Calls { get; set; }
        public IList<OldCall> OldCalls { get; set; }
    }
}