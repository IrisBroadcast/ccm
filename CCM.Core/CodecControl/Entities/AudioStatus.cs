using System.Collections.Generic;

namespace CCM.Core.CodecControl.Entities
{
    public class AudioStatus
    {
        public VuValues VuValues { get; set; }
        public List<InputStatus> InputStatuses { get; set; }
        //public List<OutputStatus> OutputStatuses { get; set; } // TODO: To be implemented
        //public List<bool> Gpis { get; set; }
        public List<bool> Gpos { get; set; }

    }
}