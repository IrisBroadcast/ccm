using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Core.Enums;

namespace CCM.Data.Entities
{
    [Table("Calls")]
    public class CallEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string SipCallID { get; set; }
        public DateTime Started { get; set; }
        public DateTime Updated { get; set; }
        public SipCallState? State { get; set; }
        public bool Closed { get; set; }
        public bool IsPhoneCall { get; set; }

        public string DlgHashId { get; set; }
        public string DlgHashEnt { get; set; }
        
        public Guid? FromId { get; set; }
        [ForeignKey("FromId")]
        public virtual RegisteredSipEntity FromSip { get; set; }
        public string FromUsername { get; set; }
        public string FromDisplayName { get; set; }
        public string FromTag { get; set; } // Används inte
        
        public Guid? ToId { get; set; }
        [ForeignKey("ToId")]
        public virtual RegisteredSipEntity ToSip { get; set; }
        public string ToUsername { get; set; }
        public string ToDisplayName { get; set; }
        public string ToTag { get; set; } // Används inte
    }
}
