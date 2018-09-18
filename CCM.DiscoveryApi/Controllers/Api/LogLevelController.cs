using System.Web.Http;
using CCM.Core.Managers;
using NLog;

namespace CCM.DiscoveryApi.Controllers.Api
{
    public class LevelModel
    {
        public string LogLevel { get; set; }
    }

    [RoutePrefix("api/loglevel")]
    public class LogLevelController : ApiController
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public LevelModel Get()
        {
            var currentLevel = LogLevelManager.GetCurrentLevel();
            log.Debug("Current log level is " + currentLevel.Name);
            return new LevelModel { LogLevel = currentLevel.Name };
        }

        public LevelModel Post(LevelModel levelModel)
        {
            if (levelModel != null)
            {
                var isSet = LogLevelManager.SetLogLevel(levelModel.LogLevel);
                if (isSet)
                {
                    log.Info("Log level changed to {0}", levelModel.LogLevel);
                }
                else
                {
                    log.Info("Log level was NOT changed.");
                }
            }

            return Get();
        }
    }
}
