using System.Collections.Generic;
using CCM.Core.Entities.Base;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class CodecType : CoreEntityWithTimestamps, ISipFilter
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<SipAccount> Users { get; set; }

        public CodecType()
        {
            Users = new List<SipAccount>();
        }
    }
}