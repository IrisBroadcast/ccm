using System;

namespace CCM.Web.Models.ApiExternal
{
    public class CodecStatus
    {
        public CodecState State { get; set; }
        public string SipAddress { get; set; }
        public string PresentationName { get; set; }
        public bool HasGpo { get; set; }
        public string ConnectedToSipAddress { get; set; }
        public string ConnectedToPresentationName { get; set; }
        public string ConnectedToLocation { get; set; }
        public bool ConnectedToHasGpo { get; set; }
        public bool IsCallingPart { get; set; }
        public DateTime CallStartedAt { get; set; }

        public override string ToString()
        {
            return State == CodecState.InCall
                ? string.Format("{0} in call with {1}", SipAddress, ConnectedToSipAddress)
                : string.Format("{0} {1}", SipAddress, State);
        }
    }
}