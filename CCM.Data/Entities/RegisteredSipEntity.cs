using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Core.Attributes;

namespace CCM.Data.Entities
{
    [Table("RegisteredSips")]
    public class RegisteredSipEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MetaProperty]
        public string SIP { get; set; }

        [MetaProperty]
        public string UserAgentHead { get; set; }

        [MetaProperty]
        public string Username { get; set; }

        [MetaProperty]
        public string DisplayName { get; set; }

        [MetaProperty]
        public string IP { get; set; }

        [MetaProperty]
        public int Port { get; set; }

        public long ServerTimeStamp { get; set; }
        public DateTime Updated { get; set; }
        public int Expires { get; set; }
        public Guid? Location_LocationId { get; set; }

        [MetaType]
        [ForeignKey("Location_LocationId")]
        public virtual LocationEntity Location { get; set; }

        public Guid? User_UserId { get; set; }

        [MetaType]
        [ForeignKey("User_UserId")]
        public virtual SipAccountEntity User { get; set; }

        public Guid? UserAgentId { get; set; }

        [MetaType]
        [ForeignKey("UserAgentId")]
        public virtual UserAgentEntity UserAgent { get; set; }
    }
}
