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
            return CreateNew(new UnitOfWorkOptions());
        }

 

        /// <summary>
        /// Begin a unit of work wrapped in the parent uow
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IUnitOfWork Create(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            var currentUow = Current;
            if (currentUow != null )
            {
                return new ChildUnitOfWork(currentUow);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }


        public IUnitOfWork Reserve(string reservationName)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            if (_ambientUnitOfWork.UnitOfWork != null &&
                _ambientUnitOfWork.UnitOfWork.IsReservedFor(reservationName))
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Reserve(reservationName);

            return unitOfWork;
        }

        public void BeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            if (!TryBeginReserved(reservationName, options))
            {
                throw new Exception($"Could not find a reserved unit of work with reservation name: {reservationName}");
            }
        }

        public bool TryBeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var uow = _ambientUnitOfWork.UnitOfWork;

            //Find reserved unit of work starting from current and going to outers
            while (uow != null && !uow.IsReservedFor(reservationName))
            {
                uow = uow.Outer;
            }

            if (uow == null)
            {
                return false;
            }

            uow.Initialize(options);

            return true;
        }


        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
            {
                uow = uow.Outer;
            }

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
             
                var outerUow = _ambientUnitOfWork.UnitOfWork;

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.SetOuter(outerUow);

                _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.SetUnitOfWork(outerUow);
                  
                };

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
