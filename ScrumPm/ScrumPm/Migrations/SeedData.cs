using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Migrations
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ScrumPMContext>();
            context.Database.EnsureCreated();

            try
            {
                context.Database.OpenConnection();

                InitTenants(context);
                InitProductOwners(context);
                InitTeams(context);
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


            var commandTextOff = "SET IDENTITY_INSERT [dbo].[Teams] OFF";
            var commandTextOn = "SET IDENTITY_INSERT [dbo].[Teams] ON";
            context.Database.ExecuteSqlCommand(commandTextOn);

            context.Teams.Add(new Team() {Id = 1, ProductOwnerId = 1, Name = "Team One"});
            context.Teams.Add(new Team() {Id = 2, ProductOwnerId = 2, Name = "Team Two"});
            context.SaveChanges();

            context.Database.ExecuteSqlCommand(commandTextOff);
        }
    }
}