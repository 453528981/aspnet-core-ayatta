using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;

namespace Ayatta.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("F://project/vscode/seller.ayatta.com/appsettings.json", optional: true);
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            }

            //builder.AddEnvironmentVariables();
            //Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();


                // Base namespace matches the resources added to the assembly from the EmbeddedResources folder.
                options.FileProviders.Add(new EmbeddedFileProvider(
                    GetType().Assembly,
                    baseNamespace: "Ayatta.Web.EmbeddedResources"));
            });
            */
            //services.AddDirectoryBrowser();
            /*
            services.AddProdLocalCache((opts) =>
            {
                opts.SyncEnable = false;
                opts.SaveInterval = 5;
                opts.CacheFile = "F://project/vscode/seller.ayatta.com/cache/dat.bin";
                opts.ConnectionString = "Server=127.0.0.1;Database=base;Uid=root;Pwd=root;charset=utf8";
            });
            */

            services.AddDefaultStorage(o =>
            {
                o.BaseConnStr = "server=127.0.0.1;database=base;uid=root;pwd=root;charset=utf8";
                o.StoreConnStr = "server=127.0.0.1;database=store;uid=root;pwd=root;charset=utf8";
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddConsole(LogLevel.Error);
            app.UseStaticFiles();

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            // }

            app.UseMvc();
        }
    }
}