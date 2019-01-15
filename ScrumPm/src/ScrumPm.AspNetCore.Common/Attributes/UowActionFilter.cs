using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.Uow;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using ScrumPm.AspNetCore.Common.Extensions;
using ScrumPm.AspNetCore.Common.Helpers;
using ScrumPm.Domain.Common.DependencyInjection;

namespace ScrumPm.AspNetCore.Common.Attributes
{
    public class UowActionFilter : IAsyncActionFilter, ITransientDependency
    {

        private readonly IUnitOfWorkFactory<IUnitOfWork> _unitOfWorkFactory;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        public UowActionFilter(IUnitOfWorkFactory<IUnitOfWork> unitOfWorkFactory, IOptions<UnitOfWorkDefaultOptions> options)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _defaultOptions = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                await next();
                return;
            }

            var methodInfo = context.ActionDescriptor.AsControllerActionDescriptor().GetMethodInfo();
            var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);

            if (unitOfWorkAttr == null || unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

      

            using (var uow = _unitOfWorkFactory.Create("default",options))
            {
                var result = await next();
                if (Succeed(result))
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
            }
        }

        private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new UnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = _defaultOptions.CalculateIsTransactional(
                    autoValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
                );
            }

            return options;
        }

        private async Task RollbackAsync(ActionExecutingContext context)
        {
            var currentUow = _unitOfWorkFactory.Create();
            await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
        }

        private static bool Succeed(ActionExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }
    }
}
