using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IStudioRepository : IRepository<Studio>
    {
        List<Studio> FindStudios(string search);
    }
}