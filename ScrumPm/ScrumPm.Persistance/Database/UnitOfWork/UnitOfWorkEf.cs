namespace ScrumPm.Persistence.Database.UnitOfWork
{
    using ScrumPm.Common.Persistence;

    public class UnitOfWorkEf<T>
        where T : class, IUnitOfWork
    {
        private readonly IContextFactory<T> _contextFactory;
        private T _dataContext;

        public UnitOfWorkEf(IContextFactory<T> databaseFactory)
        {
            this._contextFactory = databaseFactory;
        }

        protected T DataContext
        {
            get { return _dataContext ?? (_dataContext = _contextFactory.Create()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }

    }
}
