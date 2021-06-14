using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class UserAgentProfileOrder
    {
        public virtual UserAgent UserAgent { get; set; }

        public Guid UserAgentId { get; set; }

        public virtual ProfileCodec Profile { get; set; }

        public Guid ProfileId { get; set; }

        public int ProfileSortIndexForUserAgent { get; set; }
    }
}
