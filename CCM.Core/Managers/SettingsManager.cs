using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Enums;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using NLog;

namespace CCM.Core.Managers
{
    public class SettingsManager : ISettingsManager
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISettingsRepository _settingsRepository;

        public SettingsManager(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public string SipDomain => GetSetting(SettingsEnum.SIPDomain);
        public int LatestCallCount => GetSetting<int>(SettingsEnum.LatestCallCount);
        public int MaxRegistrationAge => GetSetting<int>(SettingsEnum.MaxRegistrationAge);
        public bool CodecControlActive => GetSetting<bool>(SettingsEnum.CodecControlActive);

        public List<Setting> GetSettings()
        {
            return _settingsRepository.GetAll();
        }

        public void SaveSettings(List<Setting> newSettings, string userName)
        {
            _settingsRepository.Save(newSettings, userName);
        }

        private string GetSetting(SettingsEnum enumName)
        {
            var setting = GetSettings().SingleOrDefault(s => s.Name == enumName.ToString());
            return setting == null ? string.Empty : setting.Value;
        }

        private T GetSetting<T>(SettingsEnum enumName)
        {
            try
            {
                var value = GetSetting(enumName);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Exception when reading setting {enumName} of type {typeof(T)}");
                return default(T);
            }
        }
       
    }
}