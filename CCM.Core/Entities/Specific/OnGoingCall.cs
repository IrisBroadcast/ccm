using System;

namespace CCM.Core.Entities.Specific
{
    public class OnGoingCall
    {
        public string CallId { get; set; }
        public DateTime Started { get; set; }
        public bool IsPhoneCall { get; set; }
        public string FromDisplayName { get; set; }
        public string FromSip { get; set; }
        public string FromId { get; set; }
        public string FromLocationName { get; set; }
        public string FromLocationShortName { get; set; }
        public string FromComment { get; set; }
        public string FromRegionName { get; set; }
        public string FromCodecTypeName { get; set; }
        public string FromCodecTypeColor { get; set; }
        public string ToDisplayName { get; set; }
        public string ToSip { get; set; }
        public string ToId { get; set; }
        public string ToLocationName { get; set; }
        public string ToLocationShortName { get; set; }
        public string ToComment { get; set; }
        public string ToRegionName { get; set; }
        public string ToCodecTypeName { get; set; }
        public string ToCodecTypeColor { get; set; }

        public int DurationSeconds => Convert.ToInt32(DateTime.UtcNow.Subtract(Started).TotalSeconds);

    }
}