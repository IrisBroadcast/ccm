using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{

    public class CommandIkusNetSysGetDeviceName : CommandBase
    {
        public CommandIkusNetSysGetDeviceName() : base(Command.IkusNetSysGetDeviceName, 0)
        {
        }
        
    }
    
}