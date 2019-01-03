using Microsoft.EntityFrameworkCore;
using ScrumPm.Persistence.EntityFrameworkCore;
using ScrumPm.Persistence.Teams.PersistenceModels;

namespace ScrumPm.Persistence.Database
{
    public class ScrumPmContext : EfCoreContext<ScrumPmContext>
    {
        public  DbSet<TeamEf> Teams { get; set; }
        public  DbSet<ProductOwnerEf> ProductOwners { get; set; }
        public  DbSet<TenantEf> Tenants { get; set; }



        public ScrumPmContext(DbContextOptions<ScrumPmContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TeamEf>().HasKey(c => c.Id);
            modelBuilder.Entity<TeamEf>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<TeamEf>(builder =>
            {
                builder.ToTable("Teams", "dbo");

              

            });
            modelBuilder.Entity<TeamEf>()
                .HasOne(c => c.ProductOwner)
                .WithOne(x => x.Team)
                .HasForeignKey<ProductOwnerEf>(c => c.TeamId);

            modelBuilder.Entity<ProductOwnerEf>().HasKey(c => c.Id);
            modelBuilder.Entity<ProductOwnerEf>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<ProductOwnerEf>(builder =>
            {
                builder.ToTable("ProductOwners", "dbo");

              

            });


            modelBuilder.Entity<TenantEf>().HasKey(c => c.Id);
            modelBuilder.Entity<TenantEf>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<TenantEf>(builder =>
            {
                builder.ToTable("Tenants", "dbo");

              

            });


        }
    }
}
