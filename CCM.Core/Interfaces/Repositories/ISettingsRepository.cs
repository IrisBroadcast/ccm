using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ISettingsRepository
    {
        List<Setting> GetAll();
        void Save(List<Setting> settings, string userName);
    }
}