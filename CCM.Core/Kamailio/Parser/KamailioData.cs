using System.Collections.Generic;
using CCM.Core.Kamailio.Messages;

namespace CCM.Core.Kamailio.Parser
{
    public class KamailioData
    {
        public KamailioMessageType MessageType { get; set; }
        public Dictionary<string, string> Fields { get; set; }

        public string GetField(string field) { return Fields.ContainsKey(field) ? Fields[field] : string.Empty; }
    }
}