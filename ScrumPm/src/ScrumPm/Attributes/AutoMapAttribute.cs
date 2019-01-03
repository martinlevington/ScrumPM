using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScrumPm.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        private readonly Type _sourceType;
        private readonly Type _destType;

        public AutoMapAttribute(Type sourceType, Type destType)
        {
            _sourceType = sourceType;
            _destType = destType;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var filter = new AutoMapFilter(SourceType, DestType, new Mapper() );

            filter.OnActionExecuted(context);
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type DestType
        {
            get { return _destType; }
        }
    }
}
