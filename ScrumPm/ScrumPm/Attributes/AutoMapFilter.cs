using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScrumPm.Attributes
{
    public class AutoMapFilter : IActionFilter
    {
        private readonly Type _sourceType;
        private readonly Type _destType;
        private readonly IMapper _mapper;

        public AutoMapFilter(Type sourceType, Type destType, IMapper mapper)
        {
            _sourceType = sourceType;
            _destType = destType;
            _mapper = mapper;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           // not needed
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            var model = controller.ViewData.Model;

            var viewModel = _mapper.Map(model, _sourceType, _destType);

            controller.ViewData.Model = viewModel;
        }
    }
}
