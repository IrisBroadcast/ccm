using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetGetInputEnabled : CommandBase
    {
        public CommandIkusNetGetInputEnabled() : base(Command.IkusNetGetInputEnabled, 4) {}
        
        public int Input { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            return ConvertHelper.EncodeUInt((uint)Input, bytes, offset);
        }

    }
}