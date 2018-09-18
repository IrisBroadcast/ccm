using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public abstract class ConnectCommandBase : ICommandBase
    {
        public abstract Command Command { get; }
        public abstract byte[] GetBytes();
    }
}