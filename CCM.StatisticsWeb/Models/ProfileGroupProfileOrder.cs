using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class ProfileGroupProfileOrder
    {
        public Guid ProfileGroupId { get; set; }

        public Guid ProfileId { get; set; }

        public virtual ProfileGroup ProfileGroup { get; set; }
        public virtual ProfileCodec Profile { get; set; }

        public int SortIndexForProfileInGroup { get; set; }
    }
}

