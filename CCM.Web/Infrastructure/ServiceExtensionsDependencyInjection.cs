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

using CCM.Core.Cache;
using CCM.Core.Helpers.PasswordGeneration;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Parser;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Managers;
using CCM.Core.Service;
using CCM.Core.SipEvent;
using CCM.Core.SipEvent.Parser;
using CCM.Data.Repositories;
using CCM.Web.Hubs;
using CCM.Web.Infrastructure.PasswordGeneration;
using CCM.Web.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CCM.Web.Infrastructure
{
    public static class ServiceExtensionsDependencyInjection
    {
        public static void AddGeneralDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<CodecStatusViewModelsProvider>();
            services.AddScoped<CodecInformationViewModelsProvider>();
            services.AddTransient<RegisteredUserAgentViewModelsProvider>();
            services.AddTransient<IPasswordGeneratorConfigurationProvider, PasswordGeneratorConfigurationProvider>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator>();

            services.AddTransient<ISipMessageParser, SipMessageParser>();
            services.AddTransient<ISipEventParser, SipEventParser>();

            services.AddTransient<IFilterManager, FilterManager>();
            services.AddTransient<ILocationManager, LocationManager>();
            services.AddTransient<IRegisteredCodecsManager, RegisteredCodecsManager>();
            services.AddTransient<ISettingsManager, SettingsManager>();
            services.AddTransient<ISipMessageManager, SipMessageManager>();
            services.AddTransient<IStatisticsManager, StatisticsManager>();

            services.AddTransient<ICachedCallHistoryRepository, CachedCallHistoryRepository>();
            services.AddTransient<ICallHistoryRepository, CallHistoryRepository>();

            services.AddTransient<ICachedCallRepository, CachedCallRepository>();
            services.AddTransient<ICallRepository, CallRepository>();

            services.AddTransient<ICachedLocationRepository, CachedLocationRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();

            services.AddTransient<ICachedProfileGroupRepository, CachedProfileGroupRepository>();
            services.AddTransient<IProfileGroupRepository, ProfileGroupRepository>();

            services.AddTransient<ICachedProfileRepository, CachedProfileRepository>();
            services.AddTransient<IProfileRepository, ProfileRepository>();

            services.AddTransient<ICachedRegisteredCodecRepository, CachedRegisteredCodecRepository>();
            services.AddTransient<IRegisteredCodecRepository, RegisteredCodecRepository>();

            services.AddTransient<ICachedSettingsRepository, CachedSettingsRepository>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            services.AddTransient<ICachedSipAccountRepository, CachedSipAccountRepository>();
            services.AddTransient<ISipAccountRepository, SipAccountRepository>();

            services.AddTransient<ICachedUserAgentRepository, CachedUserAgentRepository>();
            services.AddTransient<IUserAgentRepository, UserAgentRepository>();

            services.AddTransient<ICcmUserRepository, CcmUserRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ICodecTypeRepository, CodecTypeRepository>();
            services.AddTransient<IFilterRepository, FilterRepository>();
            
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IMetaRepository, MetaRepository>();
            services.AddTransient<IOwnersRepository, OwnersRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<IRegisteredCodecDetailsRepository, RegisteredCodecDetailsRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            services.AddTransient<IWebGuiHubUpdater, WebGuiHubUpdater>();
            services.AddTransient<ICodecStatusHubUpdater, CodecStatusHubUpdater>();

            // Discovery related
            services.AddScoped<IDiscoveryService, DiscoveryService>();

            // Background service
            services.AddHostedService<SipAccountService>();
        }
    }
}
