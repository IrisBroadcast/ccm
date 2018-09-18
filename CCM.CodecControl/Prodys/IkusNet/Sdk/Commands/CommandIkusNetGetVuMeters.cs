using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetGetVuMeters : CommandBase
    {
        public CommandIkusNetGetVuMeters() : base(Command.IkusNetGetVumeters, 0) {}

    }
}