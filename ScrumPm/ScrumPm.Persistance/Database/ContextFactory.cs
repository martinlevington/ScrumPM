namespace ScrumPm.Persistence.Database
{
    using System;
    using ScrumPm.Common.Persistence;

    /// <summary>
    /// Factory to Create ScrumPMContext
    /// When used with A dependancy injection container this DBContext can be scoped to a desired lifetime
    /// </summary>
    public class ContextFactory : IContextFactory<ScrumPMContext>, IDisposable
    {

        private ScrumPMContext _dataContext;

        /// <summary>
        /// Get a reference  to the ScrumPMContext.
        /// If a instance doe snot already exist a new ScrumPMContext is instantiated
        /// </summary>
        /// <returns>ScrumPMContext</returns>
        public ScrumPMContext GetContext()
        {
            return _dataContext ?? (_dataContext = new ScrumPMContext());
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
