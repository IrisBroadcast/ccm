using System;
using CCM.Core.Entities.Specific;
using CCM.Web.Models.ApiExternal;

namespace CCM.Web.Mappers
{
    public static class CodecStatusMapper
    {
        public static CodecStatus MapToCodecStatus(RegisteredSipDto rs)
        {
            return new CodecStatus
            {
                State = rs.Id == Guid.Empty ? CodecState.NotRegistered : (rs.InCall ? CodecState.InCall : CodecState.Available),
                SipAddress = rs.Sip,
                PresentationName = rs.DisplayName,
                HasGpo = rs.HasGpo,
                ConnectedToSipAddress = rs.InCallWithSip ?? string.Empty,
                ConnectedToPresentationName = rs.InCallWithName ?? string.Empty,
                ConnectedToLocation = rs.InCallWithLocation ?? string.Empty,
                ConnectedToHasGpo = rs.InCallWithHasGpo,
                IsCallingPart = rs.IsCallingPart,
                CallStartedAt = rs.CallStartedAt
            };
        }
        
    }
}