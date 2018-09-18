using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using CCM.Core.Exceptions;
using CCM.Core.Extensions;
using NLog;

namespace CCM.CodecControl.Mandozzi.Umac
{
    public class UmacClient : IDisposable
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly TcpClient _client;
        private readonly StreamWriter _streamWriter;
        private readonly StreamReader _streamReader;
        private readonly NetworkStream _stream;

        private const string Prompt = "admin>"; // Promt som kodaren skickar efter varje kommando. Består av kodarens username + hake
        private const string LineEnding = "\n\r"; // Radslut-sekvens som kodaren skickar. Lite speciell.

        public UmacClient(string ipAddress, int port)
        {
            _client = new TcpClient(ipAddress, port) { ReceiveTimeout = Sdk.Umac.ResponseTimeOut };
            _stream = _client.GetStream();

            Debug.WriteLine("ReadTimeout: " + _stream.ReadTimeout);
            Debug.WriteLine("CanTimeout: " + _stream.CanTimeout);

            _streamWriter = new StreamWriter(_stream, Encoding.ASCII, 4096, true) { AutoFlush = true };
            _streamReader = new StreamReader(_stream, Encoding.ASCII, false, 4096, true);
            Connect();
        }


        private void Connect()
        {
            /* Example dialog:
            user:
            ?
            password:
            Esc[8m*
            Welcome to the Mandozzi Elettronica Audio Over IP CLI
            Enter 'help' for a complete list of commands.
            admin> 
            */

            var s = ReadLine();

            if (s == "CLI closed" + LineEnding)
            {
                throw new UnableToConnectException("Umac says \"CLI closed\"");
            }

            if (!s.Contains("user:"))
            {
                throw new UnableToConnectException("Missing username prompt");
            }

            s = ReadUntil("?");

            WriteLine(Sdk.Umac.ExternalProtocolUserName);

            s = ReadUntil(":");

            if (!s.EndsWith(":"))
            {
                throw new UnableToConnectException("Missing password prompt");
            }

            WriteLine(Sdk.Umac.ExternalProtocolPassword);

            s = ReadUntilPrompt();
        }

        public LineStatus GetLineStatus()
        {
            var lineStatus = new LineStatus();

            WriteLine("show call");
            string s = ReadUntilPrompt();

            UmacCallState callState = new UmacCallState(s);
            lineStatus.StatusCode = callState.State;

            if (callState.State == LineStatusCode.Calling
                || callState.State == LineStatusCode.ConnectedCalled
                || callState.State == LineStatusCode.ConnectedReceived
                || callState.State == LineStatusCode.ReceivingCall)
            {
                WriteLine("show call info");
                s = ReadUntilPrompt();

                var callInfo = new UmacCallInfo(s);

                lineStatus.RemoteAddress = callInfo.ConnectedTo;
            }
            else if (callState.State == LineStatusCode.Disconnected)
            {
                lineStatus.RemoteAddress = "";
            }

            return lineStatus;
        }


        public bool Call(string callee, string profileName)
        {
            bool success;
            string s;

            // INFO: "config codec reset all" kan ta väldigt lång tid, upp mot 30 sekunder. Ibland går det dock snabbt. Behöver utredas.
            //WriteLine("config codec reset all");
            //ReadUntil(">", 60000);

            WriteLine("config codec 254 opus 48k mono 64k");
            ReadUntilPrompt();

            WriteLine("config codec 253 g722");
            ReadUntilPrompt();

            WriteLine("config codec 252 g711");
            ReadUntilPrompt();

            Write("config jb 150\r\n");
            ReadUntilPrompt();

            WriteLine(string.Format("sip {0}", callee));
            s = ReadUntilPrompt();

            // Om uppringningen misslyckas returneras "ERROR: sip call failed\n\radmin> "
            if (s.StartsWith("ERROR"))
            {
                success = false;
            }
            else
            {
                UmacCallState callState = new UmacCallState(s);
                success = callState.State == LineStatusCode.Calling;
            }

            return success;
        }

        public bool HangUp()
        {
            /* Example dialog:
                        admin> hangup
                        admin> not streaming
                        CALL_STATE disconnected [tx: idle, rx: idle]
                        */

            WriteLine("hangup");
            var s = ReadUntil(">");
            Debug.WriteLine("Hangup response: " + s);

            var success = true;

            /* The UMAC doesn't explicitly respond to the hangup command, however, the status change will
                 * be updated after the prompt is printed. We could catch that to verify that the hangup was
                 * successful.
                */
            /*
                s = client.TerminatedReadAsync("]", TimeSpan.FromMilliseconds(300)).Result;
                Match m = Regex.Match(s, @"not streaming\r\nCALL_STATE disconnected [tx: idle, rx: idle]", RegexOptions.Singleline);
                if (m.Success == false)
                {
                    success = false;
                }
                */

            return success;
        }

        private void Write(string s)
        {
            Debug.WriteLine("Writing: \"" + s + "\"");
            _streamWriter.Write(s);
        }

        private void WriteLine(string s)
        {
            Debug.WriteLine("Writing line: " + s);
            _streamWriter.Write(s + LineEnding);
        }


        private string ReadUntilPrompt(int timeoutInMilliseconds = Sdk.Umac.ResponseTimeOut)
        {
            var s = ReadUntil(Prompt, timeoutInMilliseconds);
            
            if (!s.EndsWith(Prompt))
            {
                throw new UnableToConnectException("Missing command prompt");
            }

            s = s.LeftOf(Prompt);

            return s;
        }

        private string ReadLine(int timeoutInMilliseconds = Sdk.Umac.ResponseTimeOut)
        {
            return ReadUntil(LineEnding, timeoutInMilliseconds);
        }

        private string ReadUntil(string until, int timeoutInMilliseconds = Sdk.Umac.ResponseTimeOut)
        {
            _client.ReceiveTimeout = timeoutInMilliseconds;
            var s = string.Empty;

            //Debug.WriteLine("Peek: " + _streamReader.Peek());
            //Debug.WriteLine("Data available: " + _stream.DataAvailable);

            do
            {
                var c = (char)_streamReader.Read();
                if (c == -1)
                {
                    break;
                }
                s += c;
            } while (!s.EndsWith(until));

            Debug.WriteLine("Reading until \"" + until + "\" from client. Got: " + s);
            if (!s.EndsWith(until))
            {
                throw new UnableToConnectException(string.Format("Can't read string \"{0}\" from host", until));
            }

            _client.ReceiveTimeout = Sdk.Umac.ResponseTimeOut;
            return s;
        }

        public void Dispose()
        {
            _streamReader.Close();
            _streamWriter.Close();
            _client.Close();

            _streamReader.Dispose();
            _streamWriter.Dispose();
            _client.Dispose();
        }
    }

}