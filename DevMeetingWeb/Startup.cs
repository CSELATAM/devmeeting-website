using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevMeetingWeb.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevMeetingWeb
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
            var vstsRepository = new Vsts.VstsRepository(
                Configuration["VstsAccount"],
                Configuration["VstsProject"],
                Configuration["VstsToken"]
                );

            services.Configure<MeetingManagerOptions>(Configuration.GetSection("MeetingManager"));
            
            services.AddSingleton<Vsts.VstsRepository>(vstsRepository);

            services.AddTransient<Logic.MeetingManager>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
