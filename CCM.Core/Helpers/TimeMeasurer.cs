using System;
using System.Diagnostics;
using System.Linq;
using NLog;
using NLog.Config;

namespace CCM.Core.Helpers
{
    public class TimeMeasurer : IDisposable
    {
        private readonly LogLevel _level;
        private readonly Stopwatch _stopwatch;
        private readonly string _message;
        private readonly bool _isEnabled;

        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public TimeMeasurer(string message, bool logStartMessage = false, LogLevel level = null)
        {
            _level = level ?? LogLevel.Debug;

            LoggingRule rule = LogManager.Configuration.LoggingRules.FirstOrDefault();
            _isEnabled = rule != null && rule.IsLoggingEnabledForLevel(_level);

            if (_isEnabled)
            {
                _message = message;

                if (logStartMessage)
                {
                    string s = string.Format("BEGIN:{0}", _message);
                    Log(s);
                }

                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }
        }

        private void Log(string s)
        {
            log.Log(_level, s);

            if (_level <= LogLevel.Debug)
            {
                Debug.WriteLine(s);
            }
        }

        public TimeSpan ElapsedTime { get { return _isEnabled ? _stopwatch.Elapsed : TimeSpan.Zero; } }

        public void Dispose()
        {
            if (_isEnabled)
            {
                _stopwatch.Stop();
                TimeSpan runTime = _stopwatch.Elapsed;

                string runTimeString = runTime.TotalSeconds > 1
                                           ? string.Format("{0} s", runTime.TotalSeconds)
                                           : string.Format("{0} ms", runTime.TotalMilliseconds);

                string formattedString = string.Format("END:{0} [{1}]", _message, runTimeString);
                Log(formattedString);
            }
        }
    }
}
