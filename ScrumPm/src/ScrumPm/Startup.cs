using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using ScrumPm.Application.Teams;
using ScrumPm.AspNetCore.Common.Attributes;
using ScrumPm.AspNetCore.Common.DateTimes;
using ScrumPm.Domain.Common.DateTimes;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Uow;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Middleware;
using ScrumPm.Migrations;
using ScrumPm.Persistence.Models.Teams;
using ScrumPm.Persistence.Models.Teams.Adapters;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;
using ScrumPM.Persistence.Postgres;
using ScrumPM.Persistence.Postgres.Teams.Repositories;
using ScrumPM.Persistence.Postgres.Uow;
using Serilog;

namespace ScrumPm
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
            var assemblies = DependencyContext.Default.RuntimeLibraries
                .SelectMany(library => library.GetDefaultAssemblyNames(DependencyContext.Default))
                .Select(Assembly.Load)
                .ToArray();
            new DefaultConventionalRegistrar().LoadFromAssembly(services, assemblies);
            //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetCallingAssembly());
            //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetAssembly(typeof(ApplicationBuilderExtensions)));
            //  new DefaultConventionalRegistrar().LoadFromAssembly(services, Assembly.GetAssembly(typeof(UnitOfWorkManager)));

            services.AddDbContext<ScrumPmContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ScrumPMContext")));

            services.AddScoped<UowActionFilter>();

            services.AddAutoMapper(assemblies);

            services.AddMvc(options => { options.Filters.AddService(typeof(UowActionFilter)); });

            services.AddCorrelationId();

            services.AddTransient<ITeamApplicationService, TeamApplicationService>();
            services.AddTransient<ITeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<IProductOwnerRepository, ProductOwnerRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITeamAdapterFactory, TeamAdapterFactory>();
            services.AddTransient<IPersistenceAdapter<TenantId, ProductOwnerEf, ProductOwner>, ProductOwnerAdapter>();
            services.AddScoped<IDateTimeClock, DateTimeClock>();


            services.AddScoped<IDbContextProvider<ScrumPmContext>, UnitOfWorkDbContextProvider<ScrumPmContext>>();

            services.AddScoped<IContextFactory<ScrumPmContext>, ContextFactory>();
            services.AddScoped<IUnitOfWorkFactory<IUnitOfWork>, UnitOfWorkFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                // todo change to migrations ??
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ScrumPmContext>();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    //context.Database.Migrate();

                    try
                    {
                        SeedData.Initialize(context, serviceScope.ServiceProvider);
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
                    "default",
                    "{controller=Team}/{action=Index}/{id?}");
            });
        }
    }
}