using CCM.Core.CodecControl.Enums;
using CCM.Web.Extensions;
using CCM.Web.Models.CodecControl.Base;

namespace CCM.Web.Models.CodecControl
{
    public class LineStatusViewModel : CodecViewModelBase
    {
        public string Status { get; set; }
        public string RemoteAddress { get; set; }
        public LineStatusCode LineStatus { get; set; }
        public DisconnectReason DisconnectReason { get; set; }

        public EnumDto LineStatusDto { get { return EnumDto.Create(LineStatus); } }
        public EnumDto DisconnectReasonDto { get { return EnumDto.Create(DisconnectReason); } }
    }
}