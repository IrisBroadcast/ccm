using System;
using System.Linq;
using System.Net;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Core.Managers
{
    public class LocationManager : ILocationManager
    {
        private readonly ILocationRepository _locationRepository;

        public LocationManager(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public Guid GetLocationIdByIp(string ip)
        {
            IPAddress ipAddress;
            if (!IPAddress.TryParse(ip, out ipAddress))
            {
                return Guid.Empty;
            }

            var networks = _locationRepository.GetAllLocationNetworks().Where(n => n.Network.AddressFamily == ipAddress.AddressFamily);

            Guid match = networks
                .Where(n => IPNetwork.Contains(n.Network, ipAddress))
                .OrderByDescending(n => n.Cidr)
                .Select(n => n.Id)
                .FirstOrDefault();

            return match;
        }
    }
}