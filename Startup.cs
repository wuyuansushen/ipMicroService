using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ipMicroService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IIpReflection, IpReflection>();
             services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IIpReflection ipInfo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            string locationPath = env.ContentRootPath + @"/store";
            var fileLocation = new PhysicalFileProvider(locationPath);
            app.UseStaticFiles(new StaticFileOptions() { RequestPath = "/ftp", FileProvider=fileLocation });
            app.UseDirectoryBrowser(options:(new DirectoryBrowserOptions() { RequestPath="/ftp",FileProvider=fileLocation}));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/ip", async context =>
                {
                    var XForInfo = ipInfo.GetIp();
                    var jsonOut = JsonSerializer.Serialize<XForward>(XForInfo,options:(new JsonSerializerOptions{WriteIndented=true}));
                    await context.Response.WriteAsync(jsonOut);
                });
            });
        }
    }
}
