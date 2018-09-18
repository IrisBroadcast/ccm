using System.Collections.Generic;
using CCM.Core.CodecControl.Entities;
using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class AudioStatusViewModel : CodecViewModelBase
    {
        public VuValues VuValues { get; set; }
        public List<InputStatus> InputStatuses { get; set; }
        public List<bool> Gpos { get; set; }

    }
}