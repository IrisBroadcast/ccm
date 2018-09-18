using System;
using System.Linq;
using CCM.Core.Interfaces;
using CCM.Core.Kamailio.Messages;
using NLog;

namespace CCM.Core.Kamailio.Parser
{
    public class KamailioDataParser : IKamailioDataParser
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public KamailioData ParseToKamailioData(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                log.Warn("Message body empty");
                return null;
            }

            string[] dataFields = message.Split('|');

            if (dataFields.Length == 0)
            {
                return null;
            }

            KamailioMessageType msgType;
            if (!Enum.TryParse(dataFields[0], true, out msgType))
            {
                log.Warn("Unable to get message type from {0}", dataFields[0]);
                return null;
            }

            var fieldsDictionary = dataFields
                .Select(x => x.Split(new[] { "::" }, StringSplitOptions.None))
                .Where(x => x.Length == 2 && !String.IsNullOrEmpty(x[0].Trim()))
                .ToDictionary(x => x[0].Trim(), x => x[1] == "<null>" ? string.Empty : x[1].Trim());

            return new KamailioData { MessageType = msgType, Fields = fieldsDictionary };
        }

    }
}