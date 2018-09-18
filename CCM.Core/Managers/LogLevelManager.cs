using System.Linq;
using NLog;
using NLog.Config;

namespace CCM.Core.Managers
{
    public class LogLevelManager
    {
        public static LogLevel GetCurrentLevel()
        {
            LoggingRule rule = LogManager.Configuration.LoggingRules.FirstOrDefault();
            var minLevel = rule != null ? rule.Levels.Min() ?? LogLevel.Off : LogLevel.Off;
            return minLevel;
        }

        public static bool SetLogLevel(string logLevel)
        {
            if (string.IsNullOrEmpty(logLevel))
            {
                return false;
            }

            LogLevel level = LogLevel.FromString(logLevel);

            if (level == null)
            {
                return false;
            }

            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                foreach (var l in LogLevel.AllLevels)
                {
                    if (level > l)
                        rule.DisableLoggingForLevel(l);
                    else
                        rule.EnableLoggingForLevel(l);
                }
            }

            LogManager.ReconfigExistingLoggers();
            return true;
        }
    }
}
