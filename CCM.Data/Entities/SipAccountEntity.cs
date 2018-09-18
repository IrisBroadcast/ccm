using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Core.Attributes;
using CCM.Core.Enums;
using CCM.Core.Interfaces;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("SipAccounts")]
    public class SipAccountEntity : EntityBase, ISipFilter
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Comment { get; set; }
        public string ExtensionNumber { get; set; }

        public SipAccountType AccountType { get; set; }
        public bool AccountLocked { get; set; }

        public string Password { get; set; }

        [MetaType]
        public virtual OwnerEntity Owner { get; set; }

        [MetaType]
        public virtual CodecTypeEntity CodecType { get; set; }

        public virtual ICollection<RegisteredSipEntity> RegisteredSips { get; set; }
    }
}