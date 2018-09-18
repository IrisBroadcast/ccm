using CCM.Core.CodecControl.Enums;

namespace CCM.Core.CodecControl.Entities
{
    public class LineStatus
    {
        public string RemoteAddress { get; set; }
        public LineStatusCode StatusCode { get; set; }
        public DisconnectReason DisconnectReason { get; set; }
    }
}