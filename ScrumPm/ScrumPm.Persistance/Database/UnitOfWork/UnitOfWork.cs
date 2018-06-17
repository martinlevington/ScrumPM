namespace ScrumPm.Persistence.Database.UnitOfWork
{
    using ScrumPm.Common.Persistence;

    public class UnitOfWork<T>
        where T : class, IUnitOfWork
    {
        private readonly IContextFactory<T> _contextFactory;
        private T _dataContext;

        public UnitOfWork(IContextFactory<T> databaseFactory)
        {
            this._contextFactory = databaseFactory;
        }

        protected T DataContext
        {
            get { return _dataContext ?? (_dataContext = _contextFactory.GetContext()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }

    }
}
