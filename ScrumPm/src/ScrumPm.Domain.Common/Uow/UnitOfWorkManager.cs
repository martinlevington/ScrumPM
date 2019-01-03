using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Validation;

namespace ScrumPm.Domain.Common.Uow
{
    // todo move to mvc layer
    public class UnitOfWorkManager : IUnitOfWorkManager<IUnitOfWork>, ISingletonDependency
          
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly IAmbientUnitOfWork _ambientUnitOfWork;
        private readonly IOptions<UnitOfWorkDefaultOptions> _defaultUowOptions;

        public UnitOfWorkManager(IAmbientUnitOfWork ambientUnitOfWork, IOptions<UnitOfWorkDefaultOptions> defaultUowOptions)
        {
            _ambientUnitOfWork = ambientUnitOfWork;
            _defaultUowOptions = defaultUowOptions;
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


        private IUnitOfWork CreateNewUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(_defaultUowOptions);
            _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

            return unitOfWork;
        }
    }
}
