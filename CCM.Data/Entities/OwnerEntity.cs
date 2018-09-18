using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Core.Attributes;
using CCM.Core.Interfaces;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Owners")]
    public class OwnerEntity : EntityBase, ISipFilter
    {
        [MetaProperty]
        public string Name { get; set; }

        public virtual ICollection<SipAccountEntity> Users { get; set; }
    }
}