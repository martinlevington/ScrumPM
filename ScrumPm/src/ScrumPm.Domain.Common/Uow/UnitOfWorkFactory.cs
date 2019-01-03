using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Validation;

namespace ScrumPm.Domain.Common.Uow
{
    // todo move to mvc layer
    public class UnitOfWorkManager : IUnitOfWorkManager<IUnitOfWork>, ISingletonDependency
          
    {
       

        private readonly Dictionary<string, IUnitOfWork> _unitOfWorkDictionary;
        private readonly IOptions<UnitOfWorkDefaultOptions> _defaultUowOptions;

        public UnitOfWorkManager(IOptions<UnitOfWorkDefaultOptions> defaultUowOptions)
        {
            _unitOfWorkDictionary = new Dictionary<string, IUnitOfWork>();
            _defaultUowOptions = defaultUowOptions;
        }

        public IUnitOfWork Create()
        {
            return Create("default", new UnitOfWorkOptions());
        }

        /// <summary>
        /// Create a named unit of work 
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork Create(string uowName)
        {
            return Create(uowName, new UnitOfWorkOptions());
        }


        /// <summary>
        /// Begin a unit of work wrapped in the parent uow
        /// </summary>
        /// <param name="uowName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public IUnitOfWork Create(string uowName, UnitOfWorkOptions options)
        {
            Check.NotNull(uowName, nameof(uowName));
            Check.NotNull(options, nameof(options));


            if (_unitOfWorkDictionary.ContainsKey(uowName))
            {
                return _unitOfWorkDictionary[uowName];
            }


            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            _unitOfWorkDictionary.Add(uowName, unitOfWork);

            return unitOfWork;
        }


        private IUnitOfWork CreateNewUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(_defaultUowOptions);
        
            return unitOfWork;
        }
    }
}
