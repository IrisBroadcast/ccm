using System;
using System.Text.RegularExpressions;

namespace CCM.CodecControl.Mandozzi.Umac
{
    public class UmacCallInfo
    {
        public string ConnectedTo { get;set; }
        public string State { get;set; }
        public string Rx { get;set; }
        public string Tx { get;set; }

        public UmacCallInfo(string s)
        {
            var regExp = @"\s*(?<state>[^[]+) \[(?<connectedTo>[^[]+)\].*codec tx: (?<tx>.*), rx: (?<rx>.*)";

            Match m = Regex.Match(s, regExp, RegexOptions.Singleline);
            if (m.Success == false || m.Groups.Count != 5)
            {
                throw new Exception("Unexpected response to \"show call info\".");
            }
         
            State = m.Groups["state"].Value;
            ConnectedTo = m.Groups["connectedTo"].Value;
            Tx = m.Groups["tx"].Value.Trim();
            Rx = m.Groups["rx"].Value.Trim();
        }

    }
}