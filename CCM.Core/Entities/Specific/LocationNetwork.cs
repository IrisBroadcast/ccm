using System;
using System.Net;

namespace CCM.Core.Entities.Specific
{
    public class LocationNetwork
    {
        public Guid Id { get; set; }
        public byte Cidr { get; set; }
        public IPNetwork Network { get; set; }

        public LocationNetwork(Guid id, string ipAddress, byte cidr)
        {
            Id = id;
            Cidr = cidr;
            IPNetwork network;
            IPNetwork.TryParse(ipAddress, cidr, out network);
            Network = network;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Id, Network != null ? Network.ToString() : "");
        }
    }
}