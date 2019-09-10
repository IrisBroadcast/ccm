/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CCM.Core.Cache;
using CCM.Core.Helpers.PasswordGeneration;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Kamailio;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Managers;
using CCM.Core.Service;
using CCM.Core.SipEvent;
using CCM.Core.SipEvent.Parser;
using CCM.Data.Repositories;
using CCM.Web.Infrastructure.SignalR;
using CCM.Web.Mappers;
using CCM.Web.Infrastructure.PasswordGeneration;
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
            _kernel.Bind<IKamailioMessageParser>().To<KamailioMessageParser>();
            _kernel.Bind<ISipEventParser>().To<SipEventParser>();
            _kernel.Bind<IKamailioDataParser>().To<KamailioDataParser>();

            _kernel.Bind<ISipAccountManager>().To<SipAccountManager>();
            _kernel.Bind<ICcmUserRepository>().To<CcmUserRepository>();
            _kernel.Bind<IRoleRepository>().To<RoleRepository>();
            _kernel.Bind<ISettingsManager>().To<SettingsManager>();
            _kernel.Bind<ILocationManager>().To<LocationManager>();
            _kernel.Bind<IOwnersRepository>().To<OwnersRepository>();
            _kernel.Bind<IRegionRepository>().To<RegionRepository>();
            _kernel.Bind<IFilterManager>().To<FilterManager>();
            _kernel.Bind<IFilterRepository>().To<FilterRepository>();
            _kernel.Bind<ICodecTypeRepository>().To<CodecTypeRepository>();
            _kernel.Bind<ICityRepository>().To<CityRepository>();
            _kernel.Bind<IMetaRepository>().To<MetaRepository>();
            _kernel.Bind<IStatisticsManager>().To<StatisticsManager>();
            _kernel.Bind<ICodecPresetRepository>().To<CodecPresetRepository>();
            _kernel.Bind<ILogRepository>().To<LogRepository>();

            _kernel.Bind<IGuiHubUpdater>().To<WebGuiHubUpdater>();
            _kernel.Bind<IStatusHubUpdater>().To<CodecStatusHubUpdater>();

            _kernel.Bind<IDiscoveryService>().To<DiscoveryService>();
            _kernel.Bind<ILocationInfoRepository>().To<LocationInfoRepository>();
            _kernel.Bind<IStudioRepository>().To<StudioRepository>();

            // Managers
            _kernel.Bind<IRegisteredSipsManager>().To<RegisteredSipsManager>();

            // Registered user-agents and details
            _kernel.Bind<IRegisteredSipRepository>().To<CachedRegisteredSipRepository>();
            _kernel.Bind<IRegisteredSipRepository>().To<RegisteredSipRepository>().WhenInjectedInto<CachedRegisteredSipRepository>();
            _kernel.Bind<IRegisteredSipDetailsRepository>().To<RegisteredSipDetailsRepository>();

            _kernel.Bind<ICallRepository>().To<CachedCallRepository>();
            _kernel.Bind<ICallRepository>().To<CallRepository>().WhenInjectedInto<CachedCallRepository>();

            _kernel.Bind<ICallHistoryRepository>().To<CachedCallHistoryRepository>();
            _kernel.Bind<ICallHistoryRepository>().To<CallHistoryRepository>().WhenInjectedInto<CachedCallHistoryRepository>();

            _kernel.Bind<IUserAgentRepository>().To<CachedUserAgentRepository>();
            _kernel.Bind<IUserAgentRepository>().To<UserAgentRepository>().WhenInjectedInto<CachedUserAgentRepository>();

            _kernel.Bind<ISettingsRepository>().To<CachedSettingsRepository>();
            _kernel.Bind<ISettingsRepository>().To<SettingsRepository>().WhenInjectedInto<CachedSettingsRepository>();

            _kernel.Bind<ILocationRepository>().To<CachedLocationRepository>();
            _kernel.Bind<ILocationRepository>().To<LocationRepository>().WhenInjectedInto<CachedLocationRepository>();

            _kernel.Bind<IProfileRepository>().To<CachedProfileRepository>();
            _kernel.Bind<IProfileRepository>().To<ProfileRepository>().WhenInjectedInto<CachedProfileRepository>();

            _kernel.Bind<IProfileGroupRepository>().To<CachedProfileGroupRepository>();
            _kernel.Bind<IProfileGroupRepository>().To<ProfileGroupRepository>().WhenInjectedInto<CachedProfileGroupRepository>();

            _kernel.Bind<ISipAccountRepository>().To<CachedSipAccountRepository>();
            _kernel.Bind<ISipAccountRepository>().To<SipAccountRepository>().WhenInjectedInto<CachedSipAccountRepository>();

            _kernel.Bind<IPasswordGenerator>().To<PasswordGenerator>();
            _kernel.Bind<IPasswordGeneratorConfigurationProvider>().To<PasswordGeneratorConfigurationProvider>();

            _kernel.Bind<RegisteredUserAgentViewModelsProvider>().ToSelf();
            _kernel.Bind<CodecStatusViewModelsProvider>().ToSelf();
        }
    }
}
