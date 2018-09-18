using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ICallRepository
    {
        IList<OnGoingCall> GetOngoingCalls(bool anonomize);
        bool CallExists(string callId, string hashId, string hashEnt);
        void UpdateCall(Call call);
        void CloseCall(Guid callId);
        Call GetCallBySipAddress(string sipAddress);
        CallInfo GetCallInfo(string callId, string hashId, string hashEnt);
        CallInfo GetCallInfoById(Guid callId);
    }
}
