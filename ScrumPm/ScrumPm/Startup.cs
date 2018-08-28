using System;
using AutoMapper;
using CorrelationId;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Application.Products;
using ScrumPm.Application.Teams;
using ScrumPm.Common.Persistence;
using ScrumPm.Domain.Teams;
using ScrumPm.Middleware;
using ScrumPm.Migrations;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Database.UnitOfWork;
using ScrumPm.Persistence.Teams.Repositories;
using Serilog;

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

            services.AddCorrelationId();

            services.AddTransient<ITeamApplicationService, TeamApplicationService>();
            services.AddTransient<ITeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<IProductOwnerRepository, ProductOwnerRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IUnitOfWork<ScrumPMContext>, UnitOfWorkEf<ScrumPMContext>>();
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
                    //context.Database.Migrate();
                 
                    try
                    {
                        context.Database.EnsureCreated();
                        SeedData.Initialize(serviceScope.ServiceProvider);
                    }
                    catch (Exception ex)
                    {
                  
                        Log.Error(ex, "An error occurred seeding the DB.");
                    }
                }

               
                
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCorrelationId();

            app.UseStaticFiles();
            // setup additional logging
            app.UseRemoteIpAddressLoggingMiddleware();
            app.UseMiddleware<CorrelationLoggingMiddleware>();
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
