using CCM.Core.Entities.Base;
using CCM.Core.Enums;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class SipAccount : CoreEntityBase, ISipFilter
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Comment { get; set; }
        public string ExtensionNumber { get; set; }
        public SipAccountType AccountType { get; set; }
        public bool AccountLocked { get; set; }
        public string Password { get; set; }
        public CodecType CodecType { get; set; }
        public Owner Owner { get; set; }
    }
}