using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        List<Region> FindRegions(string search);
        List<string> GetAllRegionNames();
    }
}