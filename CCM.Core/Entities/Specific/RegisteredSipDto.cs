using System;
using System.Collections.Generic;
using CCM.Core.Attributes;

namespace CCM.Core.Entities.Specific
{
    public class RegisteredSipDto
    {
        public Guid Id { get; set; }
        public DateTime Updated { get; set; }
        public string Sip { get; set; }
        public bool InCall { get; set; }
        public string DisplayName { get; set; }
        public string IpAddress { get; set; }
        public string UserAgentHeader { get; set; } // User agent-sträng kodaren skickar
        public IList<string> Profiles { get; set; }

        public List<KeyValuePair<string, string>> MetaData { get; set; }

        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
        public string CodecTypeColor { get; set; }
        public string Api { get; set; }

        // Aktiva filteregenskaper
        [FilterProperty(TableName = "Regions", ColumnName = "Name")]
        public string RegionName { get; set; }

        [FilterProperty(TableName = "CodecTypes", ColumnName = "Name")]
        public string CodecTypeName { get; set; }

        [FilterProperty(TableName = "Locations", ColumnName = "Name")]
        public string LocationName { get; set; }

        // Ej aktiva Filteregenskaper. Rensa?
        [FilterProperty(TableName = "Cities", ColumnName = "Name")]
        public string CityName { get; set; }

        [FilterProperty(TableName = "Locations", ColumnName = "ShortName")]
        public string LocationShortName { get; set; }

        [FilterProperty(TableName = "UserAgents", ColumnName = "Name")]
        public string UserAgentName { get; set; }

        public string Comment { get; set; }
        public string Image { get; set; }
        public bool HasGpo { get; set; }
        public string InCallWithId { get; set; }
        public string InCallWithSip { get; set; }
        public string InCallWithName { get; set; }
        public string InCallWithLocation { get; set; }
        public bool InCallWithHasGpo { get; set; }
        public bool IsCallingPart { get; set; } // True om i samtal och denna enhet ringde upp
        public bool IsPhoneCall { get; set; } // True om i samtal och det är ett telefonsaamtal
        public DateTime CallStartedAt { get; set; }

    }
}