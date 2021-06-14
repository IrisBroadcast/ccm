
using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StatsDbContext>(builder =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("CodecCallMonitorDatabaseContext"));
                builder.UseMySql(
                    Configuration.GetConnectionString("CodecCallMonitorDatabaseContext"),
                    options =>
                    {
                        options.EnableRetryOnFailure();
                    }).EnableSensitiveDataLogging();
            });

            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

            services.AddTransient<ICallHistoryRepository, CallHistoryRepository>();
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<ISipAccountRepository, SipAccountRepository>();
            services.AddTransient<IOwnerRepository, OwnerRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ICodecTypeRepository, CodecTypeRepository>();


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://localhost:44373")
                .AllowAnyHeader()
                .AllowAnyMethod());
            });
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
