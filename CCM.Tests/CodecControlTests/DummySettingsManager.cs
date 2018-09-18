using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Managers;

namespace CCM.Tests.CodecControlTests
{
    public class DummySettingsManager : ISettingsManager
    {
        public List<Setting> GetSettings()
        {
            throw new System.NotImplementedException();
        }

        public void SaveSettings(List<Setting> settings, string userName)
        {
            throw new System.NotImplementedException();
        }

        public string SipDomain { get; private set; }
        public int LatestCallCount { get; private set; }
        public int MaxRegistrationAge { get; private set; }
        public bool CodecControlActive { get { return true; } }
    }
}