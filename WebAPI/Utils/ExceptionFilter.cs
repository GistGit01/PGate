using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public class ExceptionFilter:IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                string message = context.Exception.Message;
                string returnCode = "FAIL";

                var response = ResultModel.Fail(message, returnCode);
                context.Result = new ContentResult
                {
                    Content = JsonSerializer.Serialize(response),
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json"
                };
            }

            context.ExceptionHandled = true; //异常已处理了

            return Task.CompletedTask;
        }
    }
}
