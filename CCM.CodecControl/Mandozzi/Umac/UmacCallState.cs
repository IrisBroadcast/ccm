using System;
using System.Text.RegularExpressions;
using CCM.Core.CodecControl.Enums;

namespace CCM.CodecControl.Mandozzi.Umac
{
    public class UmacCallState
    {
        public LineStatusCode State { get; set; }
        public string TxProtocol { get; set; }
        public string RxProtocol { get; set; }

        public UmacCallState(string s)
        {
            Match m = Regex.Match(s, @"CALL_STATE (?<state>[^[]+) \[tx: (?<tx>.*), rx: (?<rx>.*)\]");
            if (m.Success == false || m.Groups.Count != 4)
            {
                throw new Exception("Unexpected response to \"show call\".");
            }

            string state = m.Groups["state"].Value;
            switch (state)
            {
                case "unknown":
                    State = LineStatusCode.NotAvailable;
                    break;
                case "disconnected":
                    State = LineStatusCode.Disconnected;
                    break;
                case "temp_disabled":
                    State = LineStatusCode.NotAvailable;
                    break;
                case "loading voip-dsp":
                    State = LineStatusCode.Initializing;
                    break;
                case "loading bf-dsp":
                    State = LineStatusCode.Initializing;
                    break;
                case "calling":
                    State = LineStatusCode.Calling;
                    break;
                case "early":
                    State = LineStatusCode.Calling;
                    break;
                case "incoming":
                    State = LineStatusCode.ReceivingCall;
                    break;
                case "connecting":
                    State = LineStatusCode.Calling;
                    break;
                case "connected":
                    State = LineStatusCode.ConnectedCalled;
                    break;
                case "xcasting":
                    State = LineStatusCode.ConnectedCalled;
                    break;
                default:
                    State = LineStatusCode.ErrorGettingStatus;
                    break;
            }
            TxProtocol = m.Groups["tx"].Value;
            RxProtocol = m.Groups["rx"].Value;
        }
    }
}