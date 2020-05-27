using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace ScrumPM.Persistence.Postgres
{
     public interface IEfCoreDbContext : IDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable
     {
         int SaveChanges();


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        //DbSet<T> Set<T>()
        //    where T: class;

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

    }
}
