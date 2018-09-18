using System;
using System.Collections.Generic;
using CCM.Core.Entities.Base;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class Owner : CoreEntityWithTimestamps, ISipFilter
    {
        public string Name { get; set; }
        public List<SipAccount> Users { get; set; }

        public Owner()
        {
            Users = new List<SipAccount>();
        }
    }
}