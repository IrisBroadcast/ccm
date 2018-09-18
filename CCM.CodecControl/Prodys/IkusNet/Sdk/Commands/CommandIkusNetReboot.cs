using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{

    public class CommandIkusNetReboot : CommandBase
    {
        public CommandIkusNetReboot() : base(Command.IkusNetSysRebootDevice, 0)
        {
        }

    }

}