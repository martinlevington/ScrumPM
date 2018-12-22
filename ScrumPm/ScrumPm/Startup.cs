using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using ScrumPm.Application.Teams;
using ScrumPm.AspNetCore.Common.Attributes;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Uow;
using ScrumPm.Domain.Teams;
using ScrumPm.Middleware;
using ScrumPm.Migrations;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.EntityFrameworkCore;
using ScrumPm.Persistence.Teams;
using ScrumPm.Persistence.Teams.Repositories;
using ScrumPm.Persistence.Uow;
using Serilog;

namespace ScrumPm
{
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

            var assemblies = DependencyContext.Default.RuntimeLibraries
                .SelectMany(library => library.GetDefaultAssemblyNames(DependencyContext.Default))
                .Select(Assembly.Load)
               
                .ToArray();
            new DefaultConventionalRegistrar().LoadFromAssembly(services, assemblies);
          //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetCallingAssembly());
          //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetAssembly(typeof(ApplicationBuilderExtensions)));
          //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetAssembly(typeof(UnitOfWorkManager)));

            services.AddDbContext<ScrumPmContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ScrumPMContext")));

            services.AddScoped<UowActionFilter>();
          
            services.AddAutoMapper();

            services.AddMvc(options => { options.Filters.AddService(typeof(UowActionFilter)); } );

            services.AddCorrelationId();

            services.AddTransient<ITeamApplicationService, TeamApplicationService>();
            services.AddTransient<ITeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<IProductOwnerRepository, ProductOwnerRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITeamAdapterFactory, TeamAdapterFactory>();
            
            
            services.AddTransient<IDbContextProvider<ScrumPmContext>, UnitOfWorkDbContextProvider<ScrumPmContext> >();

            services.AddTransient<IContextFactory<ScrumPmContext>, ContextFactory>();
            

            

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
                    var context = serviceScope.ServiceProvider.GetRequiredService<ScrumPmContext>();
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
                    template: "{controller=Team}/{action=Index}/{id?}");
            });
        }
    }
}
