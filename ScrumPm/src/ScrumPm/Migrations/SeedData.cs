using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Migrations
{
    public static class SeedData
    {

        public static void Initialize(ScrumPmContext context , IServiceProvider serviceProvider)
        {
           
            context.Database.OpenConnection();
        

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<ScrumPmContext>();

            try
            {
              

                InitTenants(context);
                InitTeams(context);
                InitProductOwners(context);
               
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

        private static void InitTenants(ScrumPmContext context)
        {
            if (context.Tenants.Any())
            {
                return;
            }

            
          //  var commandTextOff = "SET IDENTITY_INSERT [dbo].[Tenants] OFF";
         //   var commandTextOn = "SET IDENTITY_INSERT [dbo].[Tenants] ON";
         //   context.Database.ExecuteSqlCommand(commandTextOn);

            context.Tenants.Add(new TenantEf {Id = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB"), Name = "Tenant One"});
            context.Tenants.Add(new TenantEf {Id = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87AAA"), Name = "Tenant Two"});
            context.SaveChanges();

      
         //   context.Database.ExecuteSqlCommand(commandTextOff);
          
        }

        private static void InitProductOwners(ScrumPmContext context)
        {
            if (context.ProductOwners.Any())
            {
                return;
            }

      //      var commandTextOff = "SET IDENTITY_INSERT [dbo].[ProductOwners] OFF";
       //     var commandTextOn = "SET IDENTITY_INSERT [dbo].[ProductOwners] ON";
       //     context.Database.ExecuteSqlCommand(commandTextOn);

            context.ProductOwners.Add(new ProductOwnerEf
            {
                Id = new Guid("FBAE79DB-9CA9-4776-8763-735E5EB7867B"), UserName = "bill.lone", FirstName = "Bill", LastName = "LOne", EmailAddress = "bill@email.com",
                Created = DateTime.Now, Modified = DateTime.Now,
                TeamId = new Guid("8730F86E-FD47-49EE-9356-29B4D941EA88"),
                TenantId = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB")
            });
            context.ProductOwners.Add(new ProductOwnerEf
            {
                Id = new Guid("76E6FC0D-D8BE-4235-B369-8CFF8B58558B"), UserName = "gill.ltwo", FirstName = "Gill", LastName = "LTwo", EmailAddress = "Gill@email.com",
                Created = DateTime.Now, Modified = DateTime.Now,
                TeamId = new Guid("AF34D195-13C1-4A7A-9566-11B37E82A303"),
                TenantId = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB")
            });
            context.SaveChanges();

       //     context.Database.ExecuteSqlCommand(commandTextOff);
        }


        private static void InitTeams(ScrumPmContext context)
        {
            if (context.Teams.Any())
            {
                return;
            }

           var tenantId = new Guid("544060C5-4F5F-4EA6-AC8E-7100D7E87CCB");
          //  var commandTextOff = "SET IDENTITY_INSERT [dbo].[Teams] OFF";
          //  var commandTextOn = "SET IDENTITY_INSERT [dbo].[Teams] ON";
           // context.Database.ExecuteSqlCommand(commandTextOn);

            context.Teams.Add(new TeamEf {TenantId = tenantId, Id = new Guid("8730F86E-FD47-49EE-9356-29B4D941EA88"), ProductOwnerId = new Guid("FBAE79DB-9CA9-4776-8763-735E5EB7867B"), Name = "Team One"});
            context.Teams.Add(new TeamEf {TenantId = tenantId, Id = new Guid("AF34D195-13C1-4A7A-9566-11B37E82A303"), ProductOwnerId = new Guid("76E6FC0D-D8BE-4235-B369-8CFF8B58558B"), Name = "Team Two"});
            context.SaveChanges();

           // context.Database.ExecuteSqlCommand(commandTextOff);
        }
    }
}