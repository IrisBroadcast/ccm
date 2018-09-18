using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base
{
    public abstract class CommandBase : ICommandBase 
    {
        protected Command Command;
        protected uint CommandLength;

        protected CommandBase(Command cmd, uint payloadLength)
        {
            Command = cmd;
            CommandLength = payloadLength;
        }

        public virtual byte[] GetBytes()
        {
            var bytes = new byte[CommandLength + 8];
            var offset = 0;
            offset = ConvertHelper.EncodeUInt((uint)Command, bytes, offset);
            offset = ConvertHelper.EncodeUInt(CommandLength, bytes, offset);
            EncodePayload(bytes, offset);
            return bytes;
        }

        protected virtual int EncodePayload(byte[] bytes, int offset)
        {
            return offset;
        }

    }
}