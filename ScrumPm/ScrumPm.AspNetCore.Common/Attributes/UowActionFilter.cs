using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.DependancyInjection;
using ScrumPm.Domain.Common.Uow;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ScrumPm.AspNetCore.Common.Extensions;
using ScrumPm.AspNetCore.Common.Helpers;

namespace ScrumPm.AspNetCore.Common.Attributes
{
    public class UowActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public const string UnitOfWorkReservationName = "_ActionUnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        public UowActionFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<UnitOfWorkDefaultOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
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

            context.HttpContext.Items["_ActionInfo"] = new ActionInfoInHttpContext
            {
                IsObjectResult = context.ActionDescriptor.HasObjectResult()
            };

            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            if (_unitOfWorkManager.TryBeginReserved(UnitOfWorkReservationName, options))
            {
                var result = await next();
                if (!Succeed(result))
                {
                    await RollbackAsync(context);
                }

                return;
            }

            //Begin a new, independent unit of work
            using (var uow = _unitOfWorkManager.Create(options))
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
            var currentUow = _unitOfWorkManager.Current;
            if (currentUow != null)
            {
                await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
            }
        }

        private static bool Succeed(ActionExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }
    }
}
