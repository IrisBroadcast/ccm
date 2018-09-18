using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Profiles")]
    public class ProfileEntity : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sdp { get; set; }
        public int SortIndex { get; set; }
        public virtual ICollection<ProfileGroupProfileOrdersEntity> ProfileGroups { get; set; }
        public virtual ICollection<UserAgentProfileOrderEntity> UserAgents { get; set; }
    }
}