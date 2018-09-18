using System;

namespace CCM.Web.Models.Home
{
    public class CallViewModel
    {
        public Guid CallId { get; set; }
        public string From { get; set; }
        public string FromSip { get; set; }
        public Guid FromId { get; set; }
        public string FromLocation { get; set; }
        public string FromLocationShortName { get; set; }
        public string To { get; set; }
        public string ToSip { get; set; }
        public Guid ToId { get; set; }
        public string ToLocation { get; set; }
        public string ToLocationShortName { get; set; }
        public TimeSpan Duration { get; set; }
    }
}