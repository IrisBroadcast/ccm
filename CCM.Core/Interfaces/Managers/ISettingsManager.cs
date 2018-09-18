using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Managers
{
    public interface ISettingsManager
    {
        List<Setting> GetSettings();
        void SaveSettings(List<Setting> settings, string userName);

        string SipDomain { get; }
        int LatestCallCount { get; }
        int MaxRegistrationAge { get; }
        bool CodecControlActive { get; }
    }
}