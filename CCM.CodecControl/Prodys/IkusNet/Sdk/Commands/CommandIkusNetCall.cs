using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetCall : CommandBase
    {
        public CommandIkusNetCall() : base(Command.IkusNetCall, 524) {}

        public IkusNetCodec Codec { get; set; }
        public IkusNetCallContent CallContent { get; set; }
        public IkusNetIPCallType CallType { get; set; }
        public string Profile { get; set; }
        public string Address { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            offset = ConvertHelper.EncodeUInt((uint)Codec, bytes, offset);
            offset = ConvertHelper.EncodeUInt((uint)CallContent, bytes, offset);
            offset = ConvertHelper.EncodeUInt((uint)CallType, bytes, offset);
            offset = ConvertHelper.EncodeString(Profile, bytes, offset, 256);
            offset = ConvertHelper.EncodeString(Address, bytes, offset, 256);
            return offset;
        }
    }
    
}