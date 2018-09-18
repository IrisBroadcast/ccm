using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class InputGainAndStatusViewModel : CodecViewModelBase
    {
        public bool Enabled { get; set; }
        public int GainLevel { get; set; }
    }
}