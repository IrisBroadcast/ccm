using CCM.StatisticsWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class UserAgent
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public MatchType MatchType { get; set; }
        public string Image { get; set; }

        /// <summary>
        /// Codec control part
        /// </summary>
        public string UserInterfaceLink { get; set; }

        public bool Ax { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        /// Shows link to codec user interface, for all users
        /// </summary>
        public bool UserInterfaceIsOpen { get; set; }
        public bool UseScrollbars { get; set; }
        public string Api { get; set; } // TODO: Keep this one but link it to new table, for queries that is interested.. Maybe Guid?
        public int Lines { get; set; }
        public int Inputs { get; set; }
        public int NrOfGpos { get; set; }
        public int MaxInputDb { get; set; }
        public int MinInputDb { get; set; }
        public string Comment { get; set; }
        public int InputGainStep { get; set; }
        public string GpoNames { get; set; }

        public virtual ICollection<UserAgentProfileOrder> OrderedProfiles { get; set; }
    }
}
