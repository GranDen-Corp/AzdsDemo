using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoWeb
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
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            //Note: we need to do some detection to know if it is necessary to do some configuration to prevent http => https redirect error,
            //See: https://github.com/GranDen-Corp/AzdsDemo/blob/master/DemoWeb/Startup.cs
            if (IsRunningOnK8s())
            {
                services.AddHttpsRedirection(opt => opt.HttpsPort = 443);
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;

                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (IsRunningOnK8s())
            {
                app.UseForwardedHeaders();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static bool IsRunningOnK8s()
        {
            return IsRunningInContainer() && InK8SPods();
        }

        private static bool IsRunningInContainer()
        {
            var environmentVariable = Environment.GetEnvironmentVariable(@"DOTNET_RUNNING_IN_CONTAINER");

            return environmentVariable != null && bool.Parse(environmentVariable);
        }

        private static bool InK8SPods()
        {
            var environmentVariable = Environment.GetEnvironmentVariable(@"KUBERNETES_SERVICE_HOST");
            return !string.IsNullOrEmpty(environmentVariable);
        }
    }
}
