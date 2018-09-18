using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CCM.CodecControl;
using CCM.Core.Cache;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Interfaces.Repositories.Specialized;
using CCM.Core.Kamailio;
using CCM.Core.Kamailio.Parser;
using CCM.Core.Managers;
using CCM.Core.Service;
using CCM.Data.Radius;
using CCM.Data.Repositories;
using CCM.Data.Repositories.Specialized;
using CCM.Web.Infrastructure.SignalR;
using LazyCache;
using Ninject;

namespace CCM.Web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        /// <summary>
        ///     Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        /// <summary>
        ///     Resolves multiply registered services.
        /// </summary>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IAppCache>().To<CachingService>();

            _kernel.Bind<ISipMessageManager>().To<KamailioMessageManager>();
            _kernel.Bind<ISimpleRegisteredSipRepository>().To<SimpleRegisteredSipRepository>();
            _kernel.Bind<IRegisteredSipDetailsRepository>().To<RegisteredSipDetailsRepository>();
            _kernel.Bind<IRadiusUserRepository>().To<RadiusUserRepository>();
            _kernel.Bind<ISipAccountManager>().To<SipAccountManager>();
            _kernel.Bind<ICcmUserRepository>().To<CcmUserRepository>();
            _kernel.Bind<ISipAccountRepository>().To<SipAccountRepository>();
            _kernel.Bind<IRoleRepository>().To<RoleRepository>();
            _kernel.Bind<ISettingsManager>().To<SettingsManager>();
            _kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            _kernel.Bind<IProfileGroupRepository>().To<ProfileGroupRepository>();
            _kernel.Bind<ILocationManager>().To<LocationManager>();
            _kernel.Bind<IOwnersRepository>().To<OwnersRepository>();
            _kernel.Bind<IUserAgentRepository>().To<UserAgentRepository>();
            _kernel.Bind<IRegionRepository>().To<RegionRepository>();
            _kernel.Bind<IFilterManager>().To<FilterManager>();
            _kernel.Bind<IFilterRepository>().To<FilterRepository>();
            _kernel.Bind<ICodecTypeRepository>().To<CodecTypeRepository>();
            _kernel.Bind<ICityRepository>().To<CityRepository>();
            _kernel.Bind<IMetaRepository>().To<MetaRepository>();
            _kernel.Bind<ICodecManager>().To<CodecManager>();
            _kernel.Bind<IStatisticsManager>().To<StatisticsManager>();
            _kernel.Bind<ICodecPresetRepository>().To<CodecPresetRepository>();
            _kernel.Bind<ILogRepository>().To<LogRepository>();
            _kernel.Bind<IGuiHubUpdater>().To<GuiHubUpdater>();
            _kernel.Bind<IStatusHubUpdater>().To<CodecStatusHubUpdater>();
            _kernel.Bind<ICodecInformationRepository>().To<CodecInformationRepository>();
            _kernel.Bind<IKamailioMessageParser>().To<KamailioMessageParser>();
            _kernel.Bind<IKamailioDataParser>().To<KamailioDataParser>();
            _kernel.Bind<ICallHistoryRepository>().To<CallHistoryRepository>();
            _kernel.Bind<IDiscoveryService>().To<DiscoveryService>();
            _kernel.Bind<ILocationInfoRepository>().To<LocationInfoRepository>();
            _kernel.Bind<IStudioRepository>().To<StudioRepository>();
            _kernel.Bind<IRegisteredSipRepository>().To<CachedRegisteredSipRepository>();
            _kernel.Bind<IRegisteredSipRepository>().To<RegisteredSipRepository>().WhenInjectedInto<CachedRegisteredSipRepository>();
            _kernel.Bind<ICallRepository>().To<CachedCallRepository>();
            _kernel.Bind<ICallRepository>().To<CallRepository>().WhenInjectedInto<CachedCallRepository>();
            _kernel.Bind<ISettingsRepository>().To<CachedSettingsRepository>();
            _kernel.Bind<ISettingsRepository>().To<SettingsRepository>().WhenInjectedInto<CachedSettingsRepository>();
            _kernel.Bind<ILocationRepository>().To<CachedLocationRepository>();
            _kernel.Bind<ILocationRepository>().To<LocationRepository>().WhenInjectedInto<CachedLocationRepository>();

        }
        
    }
}