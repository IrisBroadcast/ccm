using System;
using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public abstract class IkusNetStatusResponseBase
    {
        protected static byte[] GetResponseBytes(Socket socket, Command expectedCommand, int expectedResponseLength)
        {
            // Read fixed part of header
            var buffer = new byte[8];
            socket.Receive(buffer);
            var command = (Command) ConvertHelper.DecodeUInt(buffer, 0);
            var length = (int) ConvertHelper.DecodeUInt(buffer, 4);
            
            if (command != expectedCommand || length != expectedResponseLength)
            {
                throw new Exception(string.Format(
                    "Invalid response from codec. Command was {0} and length {1}. Expected {2} with length {3}",
                    command, length, expectedCommand, expectedResponseLength));
            }

            // Read variable part of header
            var result = new byte[length];
            socket.Receive(result);
            return result;
        }
    }
}