using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace CCM.Web.Hubs
{
    public class Throttler
    {
        private readonly string _idString; // Just for separating instances to make logging clearer
        private readonly int _waitTime;
        private readonly Action _action;
        private CancellationTokenSource _cancellationTokenSource;

        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public Throttler(string idString , int waitTime, Action action)
        {
            _idString = idString;
            _waitTime = waitTime;
            _action = action;
        }

        public void Trigger()
        {
            log.Trace("Throttler \"" + _idString + "\" triggered");

            if (_cancellationTokenSource != null)
            {
                log.Trace("Throttler \"" + _idString + "\" already triggered. Ignoring.");
                return;
            }

            CancellationTokenSource source = new CancellationTokenSource();

            Task.Delay(_waitTime, source.Token)
                .ContinueWith((t) =>
                {
                    log.Trace("Throttler \"" + _idString + "\" executing.");
                    _cancellationTokenSource = null;
                    _action();
                }, TaskContinuationOptions.None);

            _cancellationTokenSource = source;
        }
    }
}