using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Database
{
    using System;

    /// <summary>
    /// Factory to Create ScrumPMContext
    /// When used with A dependency injection container this DBContext can be scoped to a desired lifetime
    /// </summary>
    public class ContextFactory : IContextFactory<ScrumPMContext>, IDisposable
    {

        public ContextFactory(ScrumPMContext context)
        {
            _dataContext = context;
        }

        private ScrumPMContext _dataContext;

        /// <summary>
        /// Get a reference  to the ScrumPMContext.
        /// If a instance doe snot already exist a new ScrumPMContext is instantiated
        /// </summary>
        /// <returns>ScrumPMContext</returns>
        public ScrumPMContext Create()
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
