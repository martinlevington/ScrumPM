﻿namespace ScrumPm.Persistence.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        TDbContext GetDbContext();
    }
}
