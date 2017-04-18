using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using NLog.Web;
using NLog.Extensions.Logging;
using CityInfo.api.Services;
using Microsoft.Extensions.Configuration;
using CityInfo.api.Entities;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.api
{
    public class Startup
    {
        // This static property will hol the setting depending and can be called anywere in the app
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            /*
             * This builder will create the setting for prop "IConfigurationRoot Configuration" using json files.
             * The postion of the json files matter the top one should be all your default/developer setting and
             * any json files that change the setting on enviroment should come after. This is because the last one
             * in overwrites the default. Kind of like the cascading effect of CSS
             * 
             * Add Environment Variables is used to store sensitive information (connections string) and is called last in this que
             * also the name must match the name in the default app setting file so that it can overwrite it in the css type flow.
             * But this is not good because the enviromental variables live only on your machine and are not submitted to source control.
             * They should be added to the production sytem enviroment variable (not in the code).
            */
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                        new XmlDataContractSerializerOutputFormatter()
                    ));
            //.AddJsonOptions(o =>
            //{
            //    // This will prevent the API changing the prop names it returns, this is usually not needed but nice to know
            //    if(o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver
            //            as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;

            //    }
            //});

            // Created each time they are requested, great for lightweight, stateless services
            // services.AddTransient<>

            // Created once per request 
            // services.AddScoped<>

            // Created the first time they are requested 
            // services.AddSingleton<>

            // This now swaps the mailService passed to the controller in points of intrest depending on the build type

#if DEBUG 
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            // Best for a repository
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,  IHostingEnvironment env, ILoggerFactory loggerFactory,
            CityInfoContext cityInfoContext)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            // Seed data will not be release until Milestone 2 of EF core is release so for now we call the seed method here
            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                cfg.CreateMap<Entities.City, Models.CityDto>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfIntrestDto>();

                cfg.CreateMap<Models.PointOfIntrestCreationDto, Entities.PointOfInterest>();
            });

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
