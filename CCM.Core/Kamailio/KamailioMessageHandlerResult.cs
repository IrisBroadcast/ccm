using System;

namespace CCM.Core.Kamailio
{
    public class KamailioMessageHandlerResult
    {
        public KamailioMessageChangeStatus ChangeStatus { get; set; }
        public Guid ChangedObjectId { get; set; }
        public string SipAddress { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", ChangeStatus, ChangedObjectId);
        }

        public static KamailioMessageHandlerResult NothingChanged => new KamailioMessageHandlerResult { ChangeStatus = KamailioMessageChangeStatus.NothingChanged };
    }
}