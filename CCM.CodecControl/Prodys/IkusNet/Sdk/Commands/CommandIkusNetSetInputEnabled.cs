using System;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetSetInputEnabled : CommandBase
    {
        public CommandIkusNetSetInputEnabled() : base(Command.IkusNetSetInputEnabled, 8)
        {
        }
       
        public int Input { get; set; }
        public bool Enabled { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            offset = ConvertHelper.EncodeUInt((uint)Input, bytes, offset);
            offset = ConvertHelper.EncodeUInt(Convert.ToUInt32(Enabled), bytes, offset);
            return offset;
        }


    }
}