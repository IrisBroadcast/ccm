using CCM.StatisticsWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class SipAccount
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Comment { get; set; }
        public string ExtensionNumber { get; set; }
        public SipAccountType AccountType { get; set; }
        public bool AccountLocked { get; set; }
        public string Password { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual CodecType CodecType { get; set; }

        public virtual ICollection<RegisteredCodec> RegisteredSips { get; set; }
    }
}
