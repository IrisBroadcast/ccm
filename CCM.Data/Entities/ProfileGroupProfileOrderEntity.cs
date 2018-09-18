using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("ProfileGroupProfileOrders")]
    public class ProfileGroupProfileOrdersEntity
    {
        [Key, ForeignKey("ProfileGroup"), Column("ProfileGroup_Id", Order = 0)]
        public Guid ProfileGroupId { get; set; }

        [Key, ForeignKey("Profile"), Column("Profile_Id", Order = 1)]
        public Guid ProfileId { get; set; }

        public virtual ProfileGroupEntity ProfileGroup { get; set; }
        public virtual ProfileEntity Profile { get; set; }
        public int SortIndex { get; set; }
    }
}