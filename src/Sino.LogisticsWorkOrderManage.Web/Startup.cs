using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using Sino.CommonService;
using Sino.LogisticsWorkOrderManage.Application;
using Sino.LogisticsWorkOrderManage.Application.IServices;
using Sino.LogisticsWorkOrderManage.Application.Services;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.LogisticsWorkOrderManage.Repositories.DbContextFile;
using Sino.LogisticsWorkOrderManage.Repositories.Repositories;
using Sino.Web.Filter;

namespace Sino.LogisticsWorkOrderManage.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(GlobalResultFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDapper(Configuration.GetConnectionString("WriteConnection"), Configuration.GetConnectionString("ReadConnection"));
            services.AddDbContext<AccountInfoDbContext>(options => options.UseMySql(Configuration.GetConnectionString("WriteConnection")));

            CommonServiceConfig commonServiceConfig = new CommonServiceConfig();
            commonServiceConfig.Key = Configuration["CommonServiceAPI:appKey"];
            commonServiceConfig.Path = Configuration["CommonServiceAPI:CommonServicePath"];
            commonServiceConfig.Secret = Configuration["CommonServiceAPI:appsecret"];
            commonServiceConfig.Token = Configuration["CommonServiceAPI:Token"];
            services.AddCommonService(commonServiceConfig);

            //映射
            services.AddAutoMapper(cfg =>
            {
                AutoMapperInitialize.InitServiceMap(cfg);
            });

            services.AutoDependency(typeof(AccountInfoServices), typeof(AccountInfoRepositories));//自动注入
            services.AddTransient<IAccountInfoServices, AccountInfoServices>();
            services.AddTransient<IAccountInfoRepositories, AccountInfoRepositories>();

#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("workorder", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "V1",
                    Title = "罗基工单管理系统",
                    Description = "Api说明以及测试"
                });
            });
            services.ConfigureSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Sino.LogisticsWorkOrderManage.Web.xml"));
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Sino.LogisticsWorkOrderManage.Core.xml"));
            });
#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog($"nlog.{env.EnvironmentName}.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#if DEBUG
            //启用swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/workorder/swagger.json", "workorder");
            });
#endif

            app.UseMvc();
        }
    }
}
