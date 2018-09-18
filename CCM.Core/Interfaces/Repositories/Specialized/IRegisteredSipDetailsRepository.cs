using System;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Interfaces.Repositories.Specialized
{
    public interface IRegisteredSipDetailsRepository
    {
        RegisteredSipDetails GetRegisteredSipById(Guid id);
    }
}