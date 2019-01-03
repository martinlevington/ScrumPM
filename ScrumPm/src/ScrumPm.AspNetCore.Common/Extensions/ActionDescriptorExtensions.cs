using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using ScrumPm.Domain.Common.Threading;

namespace ScrumPm.AspNetCore.Common.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsControllerAction())
            {
                throw new Exception($"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
            }

            return actionDescriptor as ControllerActionDescriptor;
        }

        public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
        }

        public static Type GetReturnType(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetMethodInfo().ReturnType;
        }

        public static bool HasObjectResult(this ActionDescriptor actionDescriptor)
        {

           var objectResultTypes = new List<Type>
            {
                typeof(JsonResult),
                typeof(ObjectResult),
                typeof(NoContentResult)
            };

            var returnType = actionDescriptor.GetReturnType();
            returnType = AsyncHelper.UnwrapTask(returnType);

            if (!typeof(IActionResult).IsAssignableFrom(returnType))
            {
                return true;
            }

            return objectResultTypes.Any(t => t.IsAssignableFrom(returnType));

        }

        public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is ControllerActionDescriptor;
        }


       
    }
}
