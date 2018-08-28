using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ScrumPm.Attributes
{
    public class AutoMapFilterAttribute : Attribute, IFilterFactory
    {
        public Type SourceType { get; set; }
        public Type DestinationType { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // manually find and inject necessary dependencies.
            var autoMapper = serviceProvider.GetService<IMapper>();
            return new AutoMapFilter(SourceType, DestinationType,autoMapper);
        }

        public bool IsReusable => false;
    }
}
