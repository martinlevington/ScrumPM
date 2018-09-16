﻿using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Database
{
    using Microsoft.EntityFrameworkCore;

    public class ScrumPMContext : DbContext
    {
        public  DbSet<TeamEf> Teams { get; set; }
        public  DbSet<ProductOwner> ProductOwners { get; set; }
        public DbSet<Tenant> Tenants { get; set; }


        public ScrumPMContext(DbContextOptions<ScrumPMContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
