using System.Net;
using CCM.Core.Entities.Base;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class Location : CoreEntityWithTimestamps, ISipFilter
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Comment { get; set; }
        
        public string Net { get; set; } // Första IP-adress för nätet. Beskriver tillsammans med CIDR ett nätspann.
        public byte? Cidr { get; set; }

        public string Net_v6 { get; set; }
        public byte? Cidr_v6 { get; set; }

        public string CarrierConnectionId { get; set; }
        public City City { get; set; }
        public ProfileGroup ProfileGroup { get; set; }
        public Region Region { get; set; }
        
        public string ToIpV4String() { return !string.IsNullOrEmpty(Net) ? string.Format("{0} / {1}", Net, Cidr) : string.Empty; }
        public string ToIpV6String() { return !string.IsNullOrEmpty(Net_v6) ? string.Format("{0} / {1}", Net_v6, Cidr_v6) : string.Empty; }

        public byte[] ToIpV4SortString()
        {
            if (!IPAddress.TryParse(Net, out var ipAddress))
            {
                return new byte[4];
            }
            return ipAddress.GetAddressBytes();
        }

    }
}