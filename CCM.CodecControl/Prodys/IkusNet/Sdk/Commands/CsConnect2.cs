using System;
using System.Security.Cryptography;
using System.Text;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CsConnect2 : ConnectCommandBase
    {
        public override Command Command { get { return Command.CsConnect2; } }
        private string SecurityString = IkusNet.ExternalProtocolIpSecurityString;
        private string UserName = IkusNet.ExternalProtocolUserName;

        private byte[] PasswordHash
        {
            get
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    var data = Encoding.UTF8.GetBytes(IkusNet.ExternalProtocolPassword);
                    return shaM.ComputeHash(data);
                }
            }
        } 

        public override byte[] GetBytes()
        {
            var bytes = new byte[348];

            ConvertHelper.EncodeUInt((uint)Command, bytes, 0);
            ConvertHelper.EncodeUInt(340, bytes, 4);
            ConvertHelper.EncodeString(SecurityString, bytes, 8, 20);
            ConvertHelper.EncodeString(UserName, bytes, 28, 256);
            var passwordHash = PasswordHash;
            passwordHash.CopyTo(bytes,284);

            return bytes;
        }
    }
}