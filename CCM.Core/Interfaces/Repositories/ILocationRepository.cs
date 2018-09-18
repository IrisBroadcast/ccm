using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        List<Location> FindLocations(string searchString);
        List<LocationNetwork> GetAllLocationNetworks();
    }
}