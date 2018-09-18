using System;
using System.Collections.Generic;

namespace CCM.Core.Entities.Specific
{
    public class RegisteredSipDetails
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Comment { get; set; }
        public bool InCall { get; set; } // Is In Call
        public string CallWithName { get; set; }
        
        public string LocationName { get; set; }
        public string LocationComment { get; set; }
        
        public string RegionName { get; set; }
        public string CityName { get; set; }
        
        public string Ip { get; set; }
        public string Api { get; set; }
        public string UserAgentHeader { get; set; }
        public string Sip { get; set; }
        public string UserDisplayName { get; set; }
        public string UserInterfaceLink { get; set; }
        public bool UserInterfaceIsOpen { get; set; }
        public bool UseScrollbars { get; set; }
        public string Image { get; set; }
        public bool ActiveX { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Inputs { get; set; }
        public int InputMinDb { get; set; }
        public int InputMaxDb { get; set; }
        public int InputGainStep { get; set; }
        public int Lines { get; set; }
        public List<CodecPreset> CodecPresets { get; set; }
    }
}