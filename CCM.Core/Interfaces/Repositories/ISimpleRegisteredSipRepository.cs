using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ISimpleRegisteredSipRepository
    {
        RegisteredSip GetRegisteredSipById(Guid id);
        RegisteredSipInfo GetRegisteredSipInfoById(Guid id);
        List<Guid> GetRegisteredSipIdsForUser(Guid userId);
    }
}