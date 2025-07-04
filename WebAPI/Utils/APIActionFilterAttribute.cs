using Base.Repositories;
using Base.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public class APIActionFilterAttribute : ActionFilterAttribute
    {

        private (long? customerId, long? timeStamp, string? nonceString, string? sign) GetSignParamsFromContext(ActionExecutingContext context)
        {
            var requestModel = context.ActionArguments.FirstOrDefault().Value as RequestModelBase;
            if (requestModel == null)
                throw new Exception("请求格式错误");

            return (requestModel.CustomerId, requestModel.TimeStamp, requestModel.NonceString, requestModel.Sign);

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var signParams = GetSignParamsFromContext(context);

            if (signParams.customerId == null)
                throw new Exception("客户ID为空");

            var customerRepository = serviceProvider.GetService<ICustomerRepository>();
            var customer = await customerRepository.FindAsync(signParams.customerId.Value);
            if (customer == null)
                throw new Exception("客户不存在");
            else if (customer.IsDeleted)
                throw new Exception("客户已禁用");

            try
            {
                var now = DateTime.UtcNow.ToUnixTimeStamp();
                if (now - signParams.timeStamp.Value > 1000 * 300)
                    throw new Exception("签名超时");

                var verified = SignHelper.VerifySign(signParams.customerId.Value, signParams.timeStamp.Value, signParams.nonceString, customer.SecretKey, signParams.sign);
                if (!verified)
                    throw new Exception("签名验证失败");
            }
            catch (Exception ex)
            {
                throw new Exception("签名验证失败");
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
