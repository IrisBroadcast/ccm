using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Comment { get; set; }

        // IP v4
        public string Net_Address_v4 { get; set; }
        /// <summary>CIDR = Classless Inter-Domain Routing. Decides the network size.</summary>
        public byte? Cidr { get; set; }

        // IP v6
        public string Net_Address_v6 { get; set; }
        public byte? Cidr_v6 { get; set; }

        public string CarrierConnectionId { get; set; }

        public virtual City City { get; set; }

        public virtual ProfileGroup ProfileGroup { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<RegisteredCodec> RegisteredSips { get; set; }
 
        public virtual Category Category { get; set; }
    }
}

