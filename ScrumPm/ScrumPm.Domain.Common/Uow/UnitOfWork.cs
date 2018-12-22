using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.DependancyInjection;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Events;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Threading;
using ScrumPm.Domain.Common.Validation;

namespace ScrumPm.Domain.Common.Uow
{
    [ServiceRegistrationLocator(typeof(IUnitOfWork), typeof(UnitOfWork), ServiceLifetime.Scoped)]
  public class UnitOfWork : IUnitOfWork, ITransientDependency
    {
        public Guid Id { get; } = Guid.NewGuid();

        public IUnitOfWorkOptions Options { get; private set; }

        public bool IsDisposed { get; set; }
        public bool IsCompleted { get; set; }



        public IServiceProvider ServiceProvider { get; }

        private readonly Dictionary<string, IDatabaseApi> _databaseApis;
        private readonly Dictionary<string, ITransactionApi> _transactionApis;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        private Exception _exception;
        private bool _isCompleted;
        private bool _isDisposed;
        private bool _isRolledback;

        public UnitOfWork(IServiceProvider serviceProvider, IOptions<UnitOfWorkDefaultOptions> options)
        {
            ServiceProvider = serviceProvider;
            _defaultOptions = options.Value;

            _databaseApis = new Dictionary<string, IDatabaseApi>();
            _transactionApis = new Dictionary<string, ITransactionApi>();
        }

        public virtual void Initialize(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            if (Options != null)
            {
                throw new Exception("This unit of work is already initialized before!");
            }

            Options = _defaultOptions.Normalize(options.Clone());
 
        }

   

        public virtual void SaveChanges()
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                (databaseApi as ISupportsSavingChanges)?.SaveChanges();
            }
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                if (databaseApi is ISupportsSavingChanges)
                {
                    await (databaseApi as ISupportsSavingChanges).SaveChangesAsync(cancellationToken);
                }
            }
        }

        public virtual void Complete()
        {
            if (_isRolledback)
            {
                return;
            }

            PreventMultipleComplete();

            try
            {
                SaveChanges();
                CommitTransactions();
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback)
            {
                return;
            }

            PreventMultipleComplete();

            try
            {
                await SaveChangesAsync(cancellationToken);
                await CommitTransactionsAsync();
                await OnCompletedAsync();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public virtual void Rollback()
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            RollbackAll();
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            await RollbackAllAsync(cancellationToken);
        }

        public IDatabaseApi FindDatabaseApi(string key)
        {
            return _databaseApis.GetOrDefault(key);
        }

        public void AddDatabaseApi(string key, IDatabaseApi api)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(api, nameof(api));

            if (_databaseApis.ContainsKey(key))
            {
                throw new Exception("There is already a database API in this unit of work with given key: " + key);
            }

            _databaseApis.Add(key, api);
        }

        public IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(factory, nameof(factory));

            return _databaseApis.GetOrAdd(key, factory);
        }

        public ITransactionApi FindTransactionApi(string key)
        {
            Check.NotNull(key, nameof(key));

            return _transactionApis.GetOrDefault(key);
        }

        public void AddTransactionApi(string key, ITransactionApi api)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(api, nameof(api));

            if (_transactionApis.ContainsKey(key))
            {
                throw new Exception("There is already a transaction API in this unit of work with given key: " + key);
            }

            _transactionApis.Add(key, api);
        }

        public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(factory, nameof(factory));

            return _transactionApis.GetOrAdd(key, factory);
        }

    

        protected virtual void OnCompleted()
        {
            IsCompleted = true;
        }

        protected virtual async Task OnCompletedAsync()
        {
          
        }

        protected virtual void OnFailed()
        {
            
        }

        protected virtual void OnDisposed()
        {
            IsDisposed = true;
        }

        public virtual void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            DisposeTransactions();

            if (!_isCompleted || _exception != null)
            {
                OnFailed();
            }

            OnDisposed();
        }

        private void DisposeTransactions()
        {
            foreach (var transactionApi in _transactionApis.Values)
            {
                try
                {
                    transactionApi.Dispose();
                }
                catch
                {
                }
            }
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleted)
            {
                throw new Exception("Complete is called before!");
            }

            _isCompleted = true;
        }


        protected virtual void RollbackAll()
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                try
                {
                    (databaseApi as ISupportsRollback)?.Rollback();
                }
                catch { }
            }

            foreach (var transactionApi in _transactionApis.Values)
            {
                try
                {
                    (transactionApi as ISupportsRollback)?.Rollback();
                }
                catch { }
            }
        }

        protected virtual async Task RollbackAllAsync(CancellationToken cancellationToken)
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                if (!(databaseApi is ISupportsRollback))
                {
                    continue;
                }

                try
                {
                    await (databaseApi as ISupportsRollback).RollbackAsync(cancellationToken);
                }
                catch { }
            }

            foreach (var transactionApi in _transactionApis.Values)
            {
                if (!(transactionApi is ISupportsRollback))
                {
                    continue;
                }

                try
                {
                    await (transactionApi as ISupportsRollback).RollbackAsync(cancellationToken);
                }
                catch { }
            }
        }

        protected virtual void CommitTransactions()
        {
            foreach (var transaction in _transactionApis.Values)
            {
                transaction.Commit();
            }
        }

        protected virtual async Task CommitTransactionsAsync()
        {
            foreach (var transaction in _transactionApis.Values)
            {
                await transaction.CommitAsync();
            }
        }


        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}
