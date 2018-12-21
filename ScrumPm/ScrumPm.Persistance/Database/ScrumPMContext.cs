using Microsoft.EntityFrameworkCore;
using ScrumPm.Persistence.EntityFrameworkCore;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Database
{
    public class ScrumPmContext : EfCoreContext<ScrumPmContext>
    {
        public  DbSet<TeamEf> Teams { get; set; }
        public  DbSet<ProductOwnerEf> ProductOwners { get; set; }
        public DbSet<TenantEf> Tenants { get; set; }


        public ScrumPmContext(DbContextOptions<ScrumPmContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
