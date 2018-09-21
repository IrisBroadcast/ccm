using System.Collections.Generic;
using CCM.Core.Entities.Base;
using CCM.Core.Enums;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class UserAgent : CoreEntityWithTimestamps, ISipFilter
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public MatchType MatchType { get; set; }
        public string Image { get; set; }
        public string UserInterfaceLink { get; set; }
        public bool Ax { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Profile> Profiles { get; set; }
        public string Comment { get; set; }
        public string Api { get; set; }
        public int Lines { get; set; }
        public int Inputs { get; set; }
        public int NrOfGpos { get; set; }
        public int InputMinDb { get; set; }
        public int InputMaxDb { get; set; }
        public int InputGainStep { get; set; }
        public List<CodecPreset> CodecPresets { get; set; }
        public string GpoNames { get; set; }
        public bool UserInterfaceIsOpen { get; set; }
        public bool UseScrollbars { get; set; }


        public UserAgent()
        {
            Profiles = new List<Profile>();
            CodecPresets = new List<CodecPreset>();
        }
    }
}