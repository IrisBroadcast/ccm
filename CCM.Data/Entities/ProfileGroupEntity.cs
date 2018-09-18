using System.Collections.Generic;
using CCM.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("ProfileGroups")]
    public class ProfileGroupEntity : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProfileGroupProfileOrdersEntity> OrderedProfiles { get; set; }

    }
}