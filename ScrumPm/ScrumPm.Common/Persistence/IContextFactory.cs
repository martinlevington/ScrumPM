﻿namespace ScrumPm.Domain.Common.Persistence
{
    /// <summary>
    /// Context Factory Interface 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContextFactory<out T>
    {
        T Create();
    }
}
