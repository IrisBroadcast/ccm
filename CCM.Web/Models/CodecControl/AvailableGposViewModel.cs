using System.Collections.Generic;
using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class AvailableGposViewModel : CodecViewModelBase
    {
        public List<GpoViewModel> Gpos { get; set; }

        public AvailableGposViewModel()
        {
            Gpos = new List<GpoViewModel>();
        }
    }
}