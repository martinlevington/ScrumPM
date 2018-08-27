using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Application.Products;
using ScrumPm.Application.Teams;
using ScrumPm.Common.Persistence;
using ScrumPm.Domain.Teams;
using ScrumPm.Middleware;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Database.UnitOfWork;
using ScrumPm.Persistence.Teams.Repositories;

namespace ScrumPm
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ScrumPMContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ScrumPMContext")));

            services.AddAutoMapper();

            services.AddMvc();

            services.AddTransient<ITeamApplicationService, TeamApplicationService>();
            services.AddTransient<ITeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<IProductOwnerRepository, ProductOwnerRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IContextFactory<ScrumPMContext>, ContextFactory>();
            
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                // todo change to migrations ??
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ScrumPMContext>();
                    context.Database.EnsureCreated();
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            // setup additional logging
            app.UseRemoteIpAddressLoggingMiddleware();
            app.UseMiddleware<HttpContextLoggingMiddleware>();
            app.UseMiddleware<UserLoggingMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
