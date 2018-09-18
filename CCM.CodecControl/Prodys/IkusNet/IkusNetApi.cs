using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Responses;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using CCM.Core.Exceptions;
using NLog;
using CCM.CodecControl.Helpers;

namespace CCM.CodecControl.Prodys.IkusNet
{
    public class IkusNetApi : ICodecApi
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public async Task<bool> CheckIfAvailableAsync(string ip)
        {
            try
            {
                using (var socket = await GetConnectedSocketAsync(ip))
                {
                    socket.Close();
                }
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        #region Get Commands

        // Endast för test
        public async Task<string> GetDeviceNameAsync(string hostAddress)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetSysGetDeviceName());
                var response = new IkusNetGetDeviceNameResponse(socket);
                return response.DeviceName;
            }
        }

        public async Task<bool?> GetGpiAsync(string hostAddress, int gpio)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetGpi { Gpio = gpio });
                var response = new IkusNetGetGpiResponse(socket);
                return response.Active;
            }
        }

        public async Task<bool?> GetGpoAsync(string hostAddress, int gpio)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetGpo { Gpio = gpio });
                var response = new IkusNetGetGpoResponse(socket);
                return response.Active;
            }
        }

        public async Task<bool> GetInputEnabledAsync(string hostAddress, int input)
        {
            // Works only on Quantum codec, not Quantum ST
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetInputEnabled { Input = input });
                var response = new IkusNetGetInputEnabledResponse(socket);
                return response.Enabled;
            }
        }

        public async Task<int> GetInputGainLevelAsync(string hostAddress, int input)
        {
            // Works only on Quantum codec, not Quantum ST
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetInputGainLevel { Input = input });
                var response = new IkusNetGetInputGainLevelResponse(socket);
                return response.GainLeveldB;
            }
        }

        public async Task<LineStatus> GetLineStatusAsync(string hostAddress, int line)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetLineStatus { Line = (IkusNetLine)line });
                var response = new IkusNetGetLineStatusResponse(socket);

                return new LineStatus
                {
                    RemoteAddress = response.Address,
                    StatusCode = (LineStatusCode)response.LineStatus,
                    DisconnectReason = (DisconnectReason)response.DisconnectionCode,
                };
            }
        }

        public async Task<string> GetLoadedPresetNameAsync(string hostAddress, string lastPresetName)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetLoadedPresetName { LastLoadedPresetName = lastPresetName });
                var response = new IkusNetGetLoadedPresetNameResponse(socket);
                return response.PresetName;
            }
        }

        public async Task<VuValues> GetVuValuesAsync(string hostAddress)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetVuMeters());
                var response = new IkusNetGetVumetersResponse(socket);
                return new VuValues
                {
                    TxLeft = response.ProgramTxLeft,
                    TxRight = response.ProgramTxRight,
                    RxLeft = response.ProgramRxLeft,
                    RxRight = response.ProgramRxRight
                };
            }
        }

        public async Task<AudioMode> GetAudioModeAsync(string hostAddress)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                // Get encoder algoritm
                SendCommand(socket, new CommandIkusNetGetEncoderAudioMode());
                var encoderResponse = IkusNetGetEncoderAudioModeResponse.GetResponse(socket);

                // Get decoder algoritm
                SendCommand(socket, new CommandIkusNetGetDecoderAudioMode());
                var decoderResponse = IkusNetGetDecoderAudioModeResponse.GetResponse(socket);

                return new AudioMode
                {
                    EncoderAudioAlgoritm = (AudioAlgorithm)encoderResponse.AudioAlgorithm,
                    DecoderAudioAlgoritm = (AudioAlgorithm)decoderResponse.AudioAlgorithm
                };
            }
        }

        public async Task<AudioStatus> GetAudioStatusAsync(string hostAddress, int nrOfInputs, int nrOfGpos)
        {
            var audioStatus = new AudioStatus();

            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, new CommandIkusNetGetVuMeters());
                var vuResponse = new IkusNetGetVumetersResponse(socket);

                audioStatus.VuValues = new VuValues
                {
                    TxLeft = vuResponse.ProgramTxLeft,
                    TxRight = vuResponse.ProgramTxRight,
                    RxLeft = vuResponse.ProgramRxLeft,
                    RxRight = vuResponse.ProgramRxRight
                };

                audioStatus.InputStatuses = new List<InputStatus>();

                for (int input = 0; input < nrOfInputs; input++)
                {
                    SendCommand(socket, new CommandIkusNetGetInputEnabled { Input = input });
                    var enabledResponse = new IkusNetGetInputEnabledResponse(socket);
                    var inputEnabled = enabledResponse.Enabled;

                    SendCommand(socket, new CommandIkusNetGetInputGainLevel { Input = input });
                    var gainLevelResponse = new IkusNetGetInputGainLevelResponse(socket);
                    var gainLevel = gainLevelResponse.GainLeveldB;

                    audioStatus.InputStatuses.Add(new InputStatus { Enabled = inputEnabled, GainLevel = gainLevel });
                }

                //audioStatus.Gpis = new List<bool>();

                //for (int gpi = 0; gpi < nrOfGpis; gpi++)
                //{
                //    SendCommand(socket, new CommandIkusNetGetGpi { Gpio = gpi });
                //    var response = new IkusNetGetGpiResponse(socket);
                //    var gpiEnabled = response.Active;
                //    if (!gpiEnabled.HasValue)
                //    {
                //        // Indication of missing GPI for the number. Probably we passed the last one.
                //        break;
                //    }
                //    audioStatus.Gpis.Add(gpiEnabled.Value);
                //}

                audioStatus.Gpos = new List<bool>();

                for (int gpo = 0; gpo < nrOfGpos; gpo++)
                {
                    SendCommand(socket, new CommandIkusNetGetGpo { Gpio = gpo });
                    var response = new IkusNetGetGpoResponse(socket);
                    var gpoEnable = response.Active;
                    if (!gpoEnable.HasValue)
                    {
                        // Indication of missing GPO for the number. Probably we passed the last one.
                        break;
                    }
                    audioStatus.Gpos.Add(gpoEnable.Value);
                }
            }

            return audioStatus;
        }

        #endregion

        #region Configuration Commands
        public async Task<bool> CallAsync(string hostAddress, string callee, string profileName)
        {
            var cmd = new CommandIkusNetCall
            {
                Address = callee,
                Profile = profileName,
                CallContent = IkusNetCallContent.Audio,
                CallType = IkusNetIPCallType.UnicastBidirectional,
                Codec = IkusNetCodec.Program
            };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> HangUpAsync(string hostAddress)
        {
            var cmd = new CommandIkusNetHangUp { Codec = IkusNetCodec.Program };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> LoadPresetAsync(string hostAddress, string preset)
        {
            var cmd = new CommandIkusNetPresetLoad { PresetToLoad = preset };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> RebootAsync(string hostAddress)
        {
            var cmd = new CommandIkusNetReboot();
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> SetDeviceName(string hostAddress, string newDeviceName)
        {
            var cmd = new CommandIkusNetSysSetDeviceName { DeviceName = newDeviceName };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> SetGpoAsync(string hostAddress, int gpo, bool active)
        {
            var cmd = new CommandIkusNetSetGpo { Active = active, Gpo = gpo };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> SetInputEnabledAsync(string hostAddress, int input, bool enabled)
        {
            // Fungerar endast på Quantum-kodare, ej Quantum ST
            var cmd = new CommandIkusNetSetInputEnabled { Input = input, Enabled = enabled };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }

        public async Task<bool> SetInputGainLevelAsync(string hostAddress, int input, int gainLevel)
        {
            // Fungerar endast på Quantum-kodare, ej Quantum ST
            var cmd = new CommandIkusNetSetInputGainLevel { GainLeveldB = gainLevel, Input = input };
            return await SendConfigurationCommandAsync(hostAddress, cmd);
        }
        #endregion

        #region Private methods

        private async Task<bool> SendConfigurationCommandAsync(string hostAddress, ICommandBase cmd)
        {
            using (var socket = await GetConnectedSocketAsync(hostAddress))
            {
                SendCommand(socket, cmd);
                var ackResponse = new AcknowledgeResponse(socket);
                return ackResponse.Acknowleged;
            }
        }

        private int SendCommand(Socket socket, ICommandBase command)
        {
            return socket.Send(command.GetBytes());
        }

        private async Task<Socket> GetConnectedSocketAsync(string address, int sendTimeout = 300)
        {
            IPAddress ipAddress = GetIpAddress(address);
            if (ipAddress == null)
            {
                throw new UnableToResolveAddressException(string.Format("Unable to resolve ip address for {0}", address));
            }

            // Try with authenticated connect first
            // INFO: It seems that authenticated connect works also when authentication is not active on the codec. At least on some firmware versions...
            Socket connectedSocket = await ConnectAsync(ipAddress, new CsConnect2(), sendTimeout);

            if (connectedSocket != null)
            {
                return connectedSocket;
            }

            log.Warn("Unable to connect to codec at {0} using authenticated connect.", ipAddress);

            // Otherwise, try non authenticated connect
            connectedSocket = await ConnectAsync(ipAddress, new CsConnect(), sendTimeout);

            if (connectedSocket != null)
            {
                return connectedSocket;
            }

            log.Warn("Unable to connect to codec at {0}. Both authenticated and unauthenticated connect failed.", ipAddress);
            throw new UnableToConnectException();
        }

        private IPAddress GetIpAddress(string address)
        {
            if (IPAddress.TryParse(address, out var ipAddress))
            {
                return ipAddress;
            }
            var ips = Dns.GetHostAddresses(address);
            return ips.Length > 0 ? ips[0] : null;
        }

        private Task<Socket> ConnectAsync(IPAddress ipAddress, ConnectCommandBase connectCmd, int sendTimeout)
        {
            Socket socket = null;

            try
            {
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.IP);

                if (sendTimeout > 0)
                {
                    socket.SendTimeout = sendTimeout;
                }

                var endpoint = new IPEndPoint(ipAddress, Sdk.IkusNet.ExternalProtocolIpCommandsPort);
                socket.Connect(endpoint, TimeSpan.FromMilliseconds(1000));

                if (!socket.Connected)
                {
                    socket.Close();
                    return Task.FromResult<Socket>(null);
                }

                var sent = SendCommand(socket, connectCmd);

                if (sent <= 0 || !socket.Connected)
                {
                    socket.Close();
                    return Task.FromResult<Socket>(null);
                }

                var ackResponse = new AcknowledgeResponse(socket);
                log.Debug("Connect response from codec at {0}: {1}", ipAddress, ackResponse);

                var success = ackResponse.Acknowleged;

                if (!success)
                {
                    socket.Close();
                    return Task.FromResult<Socket>(null);
                }

                return Task.FromResult(socket);
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Exception when connecting to codec at {0}", ipAddress);
                socket?.Close();
                return null;
            }
        }

        #endregion


    }
}