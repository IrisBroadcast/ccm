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

using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using CCM.Core.Cache;
using CCM.Core.Helpers;
using CCM.Core.Helpers.PasswordGeneration;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Parser;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Managers;
using CCM.Core.Service;
using CCM.Core.SipEvent;
using CCM.Core.SipEvent.Parser;
using CCM.Data;
using CCM.Data.Repositories;
using CCM.Web.Hubs;
using CCM.Web.Infrastructure;
using CCM.Web.Infrastructure.PasswordGeneration;
using CCM.Web.Mappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace CCM.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Application deploy information
            services.Configure<ApplicationBuildInformation>(Configuration.GetSection("Build"));

            // Register cache
            services.AddLazyCache();

            // Localization
            // AddLocalization adds the localization services to the services container.
            // The code above also sets the resources path to "Resources".
            //services.AddLocalization(o => o.ResourcesPath = "Resources");
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // State what the default culture for your application is.
                // This will be used if no specific culture can be determined for
                // a given request.
                options.DefaultRequestCulture = new RequestCulture("sv-SE", "sv-SE");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting
                // numbers, dates, etc.
                var supportedCultures = new[]
                {
                    new CultureInfo("sv-SE")
                };
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings,
                // i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

            // Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, SipAccountBasicAuthenticationAttribute>("SipAccountBasicAuthentication", null)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // A cookie authentication scheme constructing the user's identity from cookies.
                    // - Challenge: A cookie authentication scheme redirecting the user to a login page.
                    // - Forbid: A cookie authentication scheme redirecting the user to a page indicating access was forbidden.
                    Configuration.Bind("CookieSettings", options);
                    options.LoginPath = new PathString("/Account/Login/");
                    options.LogoutPath = new PathString("/Account/LogOff/");
                    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // A JWT bearer scheme deserializing and validating a JWT bearer token to construct the user's identity.
                    // - Challenge: A JWT bearer scheme returning a 401 result with a www-authenticate: bearer header.
                    // - Forbid: A JWT bearer scheme returning a 403 result.
                    Configuration.Bind("JwtSettings", options);
                });

            // Custom CCM Accounts policies
            services.AddAuthorization(options =>
            {
                // Use these policies through attribues [Authorize(Policy = CCM.Core.Helpers.Roles.Admin)]
                options.AddPolicy("SipAccountBasicAuthentication", policy =>
                {
                    policy.AddAuthenticationSchemes("SipAccountBasicAuthentication");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(CCM.Core.Helpers.Roles.Admin,
                    (policy) => policy.RequireRole(CCM.Core.Helpers.Roles.Admin));
                options.AddPolicy(CCM.Core.Helpers.Roles.Remote,
                    (policy) => policy.RequireRole(CCM.Core.Helpers.Roles.Remote));
                options.AddPolicy(CCM.Core.Helpers.Roles.AccountManager,
                    (policy) => policy.RequireRole(CCM.Core.Helpers.Roles.AccountManager));
            });

            // Form default max size
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue; //4096
            });

            // SignalR / WebSockets for Hubs
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Add Cross Origin support
            services.AddCors();

            // Register services
            services.AddGeneralDependencyInjection();

            // Add database server DI
            services.AddDbContext<CcmDbContext>(builder =>
            {
                builder.UseMySql(
                    Configuration.GetConnectionString("CodecCallMonitorDatabaseContext"),
                    options =>
                    {
                        options.EnableRetryOnFailure();
                    })
                    .EnableDetailedErrors(false)
                    .EnableSensitiveDataLogging(false);
            });

            // AddDataAnnotationsLocalization adds support for localized DataAnnotations
            // validation messages through IStringLocalizer abstractions.
            services.AddControllersWithViews().AddDataAnnotationsLocalization().AddViewLocalization().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this
                // for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Static files for serving images and static files in wwwroot.
            app.UseStaticFiles();
            var uapath = Configuration.GetValue<string>("UserAgentImagesFolder");
            if (!string.IsNullOrWhiteSpace(uapath) && Directory.Exists(uapath))
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(uapath),
                    RequestPath = "/uaimages"
                });
            }

            // Localization (RFC 4646)
            app.UseRequestLocalization();

            app.UseRouting();

            // Authentication is a process in which a user provides credentials that are
            // then compared to those stored in an operating system, database, app or resource.
            // Needs to be defined after 'UseRouting'.
            app.UseAuthentication();

            // The authorization refers to the process that determines what a user is allowed to do.
            app.UseAuthorization();

            //app.UseRequestResponseLogging();

            // Set global CORS policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseEndpoints(endpoints =>
            {
                // Mvc routes
                endpoints.MapControllerRoute(
                    name: "DefaultMvc",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );

                // Api routes
                endpoints.MapControllerRoute(
                    name: "DefaultApiActionRoute",
                    pattern: "api/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "DefaultApiRoute",
                    pattern: "api/{controller}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );

                // SignalR / Websocket routes
                endpoints.MapHub<WebGuiHub>("/webguihub");
                endpoints.MapHub<CodecStatusHub>("/codecstatushub");
            });

        }
    }

    public static class ServiceExtensionsDependencyInjection {
        public static void AddGeneralDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<CodecStatusViewModelsProvider>();
            services.AddScoped<CodecInformationViewModelsProvider>();

            services.AddTransient<RegisteredUserAgentViewModelsProvider>();
            services.AddTransient<IPasswordGeneratorConfigurationProvider, PasswordGeneratorConfigurationProvider>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator>();

            services.AddTransient<IKamailioEventParser, KamailioEventParser>();
            services.AddTransient<ISipEventParser, SipEventParser>();

            services.AddTransient<IFilterManager, FilterManager>();
            services.AddTransient<ILocationManager, LocationManager>();
            services.AddTransient<IRegisteredCodecsManager, RegisteredCodecsManager>();
            services.AddTransient<ISettingsManager, SettingsManager>();
            services.AddTransient<IStatisticsManager, StatisticsManager>();

            services.AddTransient<ISipMessageManager, SipMessageManager>();
            services.AddTransient<IExternalStoreMessageManager, ExternalStoreMessageManager>();

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

    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
