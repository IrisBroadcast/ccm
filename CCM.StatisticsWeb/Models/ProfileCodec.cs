using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class ProfileCodec
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Sdp { get; set; }

        public virtual ICollection<ProfileGroupProfileOrder> ProfileGroups { get; set; }

        public virtual ICollection<UserAgentProfileOrder> UserAgents { get; set; }
    }
}
