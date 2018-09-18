using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        List<CcmRole> GetRoles();

    }
}