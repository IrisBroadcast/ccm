using System.Collections.Generic;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Interfaces.Repositories.Specialized
{
    public interface ILocationInfoRepository
    {
        List<LocationInfo> GetAll();
    }
}