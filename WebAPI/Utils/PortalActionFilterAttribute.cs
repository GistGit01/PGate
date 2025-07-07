using Base.Models;
using Base.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public class PortalActionFilterAttribute:ActionFilterAttribute
    {
        public bool IsAnonymous { get; set; } = false;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IsAnonymous)
            {
                var requestModel = context.ActionArguments.FirstOrDefault().Value as PortalRequestModelBase;
                if (requestModel == null)
                    throw new Exception("请求格式错误");

                var token = requestModel.Token;

                var serviceProvider = context.HttpContext.RequestServices;

                var cache = serviceProvider.GetService<IMemoryCache>();
                var customer = cache.Get<Customer>(token);
                if (customer == null)
                    throw new Exception("Token错误，请登录后访问");

                ((PortalBaseController)context.Controller).Customer = customer;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
