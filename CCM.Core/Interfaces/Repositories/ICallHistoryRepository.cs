using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ICallHistoryRepository
    {
        bool Save(CallHistory callHistory);
        CallHistory GetById(Guid id);
        CallHistory GetCallHistoryByCallId(Guid callId);
        IList<CallHistory> GetCallHistories(DateTime startTime, DateTime endTime);
        IList<OldCall> GetOldCalls(int callCount, bool anonymize);
        IList<OldCall> GetOldCallsFiltered(string region, string codecType, string sipAddress, string searchString, bool anonymize, bool onlyPhoneCalls, int callCount);
        IList<CallHistory> GetCallHistoriesForRegion(DateTime startDate, DateTime endDate, Guid regionId);
        IList<CallHistory> GetCallHistoriesForRegisteredSip(DateTime startDate, DateTime endDate, string sipId);
        IList<CallHistory> GetCallHistoriesForCodecType(DateTime startDate, DateTime endDate, Guid codecTypeId);
        IList<CallHistory> GetCallHistoriesForLocation(DateTime startDate, DateTime endDate, Guid locationId);
    }
}