namespace CCM.Core.Kamailio.Messages
{
    public class KamailioDialogMessage : KamailioMessageBase
    {
        public DialogStatus Status { get; set; }
        public string CallId { get; set; }
        public string HashId { get; set; }
        public string HashEntry { get; set; }
        public SipUri FromSipUri { get; set; }
        public SipUri ToSipUri { get; set; }
        public string FromDisplayName { get; set; }
        public string ToDisplayName { get; set; }
        public string FromTag { get; set; }
        public string ToTag { get; set; }
        public string Sdp { get; set; }
        public string HangupReason { get; set; }

        public override string ToDebugString()
        {
            if (Status == DialogStatus.SingleBye)
            {
                return string.Format("CallId:{0}, FromSip:{1}, ToSip:{2}, FromTag:{3}, ToTag:{4}",
                    CallId, 
                    FromSipUri != null ? FromSipUri.UserAtHost : string.Empty,
                    ToSipUri != null ? ToSipUri.UserAtHost : string.Empty,
                    FromTag, 
                    ToTag);
            }

            if (Status == DialogStatus.End)
            {
                return string.Format("CallId:{0}, HashId:{1}, HashEntry:{2}, Hangup reason:{3}, FromDisplayName:{4} FromSip:{5}, ToDisplayName:{6} ToSip:{7}, FromTag:{8}, ToTag:{9}",
                    CallId, HashId, HashEntry, HangupReason,
                    FromDisplayName, FromSipUri != null ? FromSipUri.UserAtHost : string.Empty,
                    ToDisplayName, ToSipUri != null ? ToSipUri.UserAtHost : string.Empty,
                    FromTag, ToTag);

            }

            return string.Format("CallId:{0}, HashId:{1}, HashEntry:{2}, FromDisplayName:{3} FromSip:{4}, ToDisplayName:{5} ToSip:{6}, FromTag:{7}, ToTag:{8}", 
                CallId, HashId, HashEntry,
                FromDisplayName, FromSipUri != null ? FromSipUri.UserAtHost : string.Empty, 
                ToDisplayName, ToSipUri != null ? ToSipUri.UserAtHost : string.Empty, 
                FromTag, ToTag);
        }
    }
}