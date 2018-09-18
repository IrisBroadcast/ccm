using System;

namespace CCM.Core.Interfaces.Managers
{
    public interface ILocationManager
    {
        Guid GetLocationIdByIp(string ip);
    }
}