using System;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CsConnect : ConnectCommandBase
    {
        public override Command Command { get { return Command.CsConnect; } }
        private string SecurityString = IkusNet.ExternalProtocolIpSecurityString;

        public override byte[] GetBytes()
        {
            var bytes = new byte[28];

            var count = 0;
            ConvertHelper.EncodeUInt((uint)Command, bytes, 0);
            ConvertHelper.EncodeUInt(20, bytes, 4);
            ConvertHelper.EncodeString(SecurityString, bytes, 8, 20);

            Array.Clear(bytes, count, bytes.Length - count);

            return bytes;
        }
    }
}