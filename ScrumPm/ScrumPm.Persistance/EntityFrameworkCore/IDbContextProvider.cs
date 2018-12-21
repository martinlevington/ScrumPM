using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPm.Persistence.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        TDbContext GetDbContext();
    }
}
