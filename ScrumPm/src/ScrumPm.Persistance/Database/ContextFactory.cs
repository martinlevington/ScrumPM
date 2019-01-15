using System;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Database
{
    /// <summary>
    /// Factory to Create ScrumPMContext
    /// When used with A dependency injection container this DBContext can be scoped to a desired lifetime
    /// </summary>
    public class ContextFactory : IContextFactory<ScrumPmContext>, IDisposable
    {

        public ContextFactory(ScrumPmContext context)
        {
            _dataContext = context;
        }

        private ScrumPmContext _dataContext;

        /// <summary>
        /// Get a reference  to the ScrumPMContext.
        /// If a instance doe snot already exist a new ScrumPMContext is instantiated
        /// </summary>
        /// <returns>ScrumPMContext</returns>
        public ScrumPmContext Create()
        {
            return _dataContext;
        }

        /// <summary>
        /// Dispose the ContextFactory
        /// </summary>
        public void Dispose()
        {
            _dataContext?.Dispose();
        }
    }
}
