using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ScrumPm.AspNetCore.Common.Attributes;
using ScrumPm.Domain.Common.Uow;

namespace ScrumPm.AspNetCore.Common.Middleware
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkMiddleware(RequestDelegate next, IUnitOfWorkManager unitOfWorkManager)
        {
            _next = next;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var uow = _unitOfWorkManager.Reserve(UowActionFilter.UnitOfWorkReservationName))
            {
                await _next(httpContext);
                await uow.CompleteAsync(httpContext.RequestAborted);
            }
        }
    }
}
