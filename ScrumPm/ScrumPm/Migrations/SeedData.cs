using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;
using Tenant = ScrumPm.Persistence.Teams.PersistenceModels.Tenant;

namespace ScrumPm.Migrations
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ScrumPMContext>();
            context.Database.EnsureCreated();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<ScrumPMContext>();

            try
            {
                context.Database.OpenConnection();

                InitTenants(context);
                InitProductOwners(context);
                InitTeams(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex ,"Failed to init or seed DB");
            }
            finally
            {
                context.Database.CloseConnection();
            }

        }

        private static void InitTenants(ScrumPMContext context)
        {
            if (context.Tenants.Any())
            {
                return;
            }

            
            var commandTextOff = "SET IDENTITY_INSERT [dbo].[Tenants] OFF";
            var commandTextOn = "SET IDENTITY_INSERT [dbo].[Tenants] ON";
            context.Database.ExecuteSqlCommand(commandTextOn);

            context.Tenants.Add(new Tenant() {Id = 1, Name = "Tenant One"});
            context.Tenants.Add(new Tenant() {Id = 2, Name = "Tenant Two"});
            context.SaveChanges();

      
            context.Database.ExecuteSqlCommand(commandTextOff);
          
        }

        private static void InitProductOwners(ScrumPMContext context)
        {
            if (context.ProductOwners.Any())
            {
                return;
            }

            var commandTextOff = "SET IDENTITY_INSERT [dbo].[ProductOwners] OFF";
            var commandTextOn = "SET IDENTITY_INSERT [dbo].[ProductOwners] ON";
            context.Database.ExecuteSqlCommand(commandTextOn);

            context.ProductOwners.Add(new ProductOwner()
            {
                Id = 1, UserName = "bill.lone", FirstName = "Bill", LastName = "LOne", EmailAddress = "bill@email.com",
                Created = DateTime.Now, Modified = DateTime.Now
            });
            context.ProductOwners.Add(new ProductOwner()
            {
                Id = 2, UserName = "gill.ltwo", FirstName = "Gill", LastName = "LTwo", EmailAddress = "Gill@email.com",
                Created = DateTime.Now, Modified = DateTime.Now
            });
            context.SaveChanges();

            context.Database.ExecuteSqlCommand(commandTextOff);
        }


        private static void InitTeams(ScrumPMContext context)
        {
            if (context.Teams.Any())
            {
                return;
            }

           var tenantId = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB");
          //  var commandTextOff = "SET IDENTITY_INSERT [dbo].[Teams] OFF";
          //  var commandTextOn = "SET IDENTITY_INSERT [dbo].[Teams] ON";
           // context.Database.ExecuteSqlCommand(commandTextOn);

            context.Teams.Add(new TeamEf() {TenantId = tenantId, Id = new Guid("8730F86E-FD47-49EE-9356-29B4D941EA88"), ProductOwnerId = 1, Name = "Team One"});
            context.Teams.Add(new TeamEf() {TenantId = tenantId, Id = new Guid("AF34D195-13C1-4A7A-9566-11B37E82A303"), ProductOwnerId = 2, Name = "Team Two"});
            context.SaveChanges();

           // context.Database.ExecuteSqlCommand(commandTextOff);
        }
    }
}