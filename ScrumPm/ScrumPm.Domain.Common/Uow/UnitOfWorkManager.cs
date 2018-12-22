using System;
using Microsoft.Extensions.DependencyInjection;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Validation;

namespace ScrumPm.Domain.Common.Uow
{

    [ServiceRegistrationLocator(typeof(IUnitOfWorkManager), typeof(UnitOfWorkManager))]
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(
            IServiceProvider serviceProvider,
            IAmbientUnitOfWork ambientUnitOfWork)
        {
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
        }

        /// <summary>
        /// Create a new unit of work
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IUnitOfWork CreateNew(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }

        /// <summary>
        /// Begin a unit of work wrapped in the parent uow
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IUnitOfWork Create()
        {
            return Create(new UnitOfWorkOptions());
        }

 

        /// <summary>
        /// Begin a unit of work wrapped in the parent uow
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IUnitOfWork Create(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

      
            if (Current != null )
            {
               return  Current;
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }




        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;


            return uow;
        }

        /// <summary>
        /// Wrap the unit of work in a scope
        /// </summary>
        /// <returns></returns>
        private IUnitOfWork CreateNewUnitOfWork()
        {
            var scope = _serviceProvider.CreateScope();
            try
            {
             

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
            finally
            {
               // scope.Dispose();
            }
        }
    }
}
