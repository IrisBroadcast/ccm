using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class VuValuesViewModel : CodecViewModelBase
    {
        public int RxLeft { get; set; }
        public int RxRight { get; set; }
        public int TxLeft { get; set; }
        public int TxRight { get; set; }
    }
}