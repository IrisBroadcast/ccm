using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("UserAgentProfileOrders")]
    public class UserAgentProfileOrderEntity
    {
        [Key, ForeignKey("UserAgent"), Column("UserAgentId", Order = 0)]
        public Guid UserAgentId { get; set; }

        [Key, ForeignKey("Profile"), Column("ProfileId", Order = 1)]
        public Guid ProfileId { get; set; }

        public virtual UserAgentEntity UserAgent { get; set; }
        public virtual ProfileEntity Profile { get; set; }
        public int SortIndex { get; set; }
    }
}