using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScrumPm.AspNetCore.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        private readonly Type _sourceType;
        private readonly Type _destType;
        private readonly IMapper _mapper;

        public AutoMapAttribute(Type sourceType, Type destType, IMapper mapper)
        {
            _sourceType = sourceType;
            _destType = destType;
            _mapper = mapper;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var filter = new AutoMapFilter(SourceType, DestType,  _mapper );

            filter.OnActionExecuted(context);
        }

        public Type SourceType => _sourceType;

        public Type DestType => _destType;
    }
}
