using System;
using CCM.Core.Entities.Base;
using CCM.Core.Enums;

namespace CCM.Core.Entities
{
    public class Call : CoreEntityBase
    {
        public string CallId { get; set; } // Id från Kamailio
        public string DlgHashId { get; set; }
        public string DlgHashEnt { get; set; }
        
        public Guid FromId { get; set; }
        public RegisteredSip From { get; set; }
        public string FromSip { get; set; }
        public string FromDisplayName { get; set; }
        
        public Guid ToId { get; set; }
        public RegisteredSip To { get; set; }
        public string ToSip { get; set; }
        public string ToDisplayName { get; set; }
        
        public DateTime Started { get; set; }
        public DateTime Updated { get; set; }
        
        public SipCallState State { get; set; } // Verkar inte användas. Utred och ta bort.
        public bool Closed { get; set; }
        public bool IsPhoneCall { get; set; } // True om det är ett telefonsamtal

        public string ToTag { get; set; } // Används inte
        public string FromTag { get; set; } // Används inte
    }
}