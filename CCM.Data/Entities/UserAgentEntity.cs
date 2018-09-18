using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Core.Attributes;
using CCM.Core.Enums;
using CCM.Core.Interfaces;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    // Kodarmodell
    [Table("UserAgents")]
    public class UserAgentEntity : EntityBase, ISipFilter
    {
        [MetaType]
        public string Name { get; set; }

        public string Identifier { get; set; }
        public MatchType MatchType { get; set; }
        public string Image { get; set; }
        public string UserInterfaceLink { get; set; }

        /// <summary>
        /// True if this UserAgent uses ActiveX for web interface.
        /// </summary>
        public bool Ax { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Api { get; set; }
        public int Lines { get; set; }
        public int Inputs { get; set; }
        public int MaxInputDb { get; set; }
        public int MinInputDb { get; set; }
        public string Comment { get; set; }
        public int InputGainStep { get; set; }
        public string GpoNames { get; set; }
        public bool UserInterfaceIsOpen { get; set; }
        public bool UseScrollbars { get; set; }
        public virtual ICollection<UserAgentProfileOrderEntity> OrderedProfiles { get; set; }
        public virtual ICollection<RegisteredSipEntity> RegisteredSips { get; set; }
        public virtual ICollection<CodecPresetEntity> CodecPresets { get; set; }
    }
}