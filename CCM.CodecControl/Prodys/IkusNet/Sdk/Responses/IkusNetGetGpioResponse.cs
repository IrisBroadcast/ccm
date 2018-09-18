using System;
using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetGpiResponse : IkusNetGetGpioResponse
    {
        public IkusNetGetGpiResponse(Socket socket) :base(socket, Command.IkusNetGetGpi)
        {
        }
    }

    public  class IkusNetGetGpoResponse : IkusNetGetGpioResponse
    {
        public IkusNetGetGpoResponse(Socket socket) : base(socket, Command.IkusNetGetGpo)
        {
        }
    }

    public abstract class IkusNetGetGpioResponse : IkusNetStatusResponseBase
    {
        public bool? Active { get; protected set; }

        protected IkusNetGetGpioResponse(Socket socket, Command command)
        {
            ParseResponse(socket, command);
        }

        protected void ParseResponse(Socket socket, Command expectedCommand)
        {
            var buffer = new byte[8];
            socket.Receive(buffer);
            var command = (Command) ConvertHelper.DecodeUInt(buffer, 0);
            var length = (int) ConvertHelper.DecodeUInt(buffer, 4);

            if (command != expectedCommand || length != 4)
            {
                // This is usually a sign that no GPIO exists for the requested gpio number
                Active = null;
                return;
            }

            var payloadBytes = new byte[length];
            socket.Receive(payloadBytes);

            Active = Convert.ToBoolean(ConvertHelper.DecodeUInt(payloadBytes, 0));
        }
        
    }
}