using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Usermanger.Filter
{
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<MyExceptionFilter> logger;
        private readonly IHostEnvironment env;

        public MyExceptionFilter(IHostEnvironment env,ILogger<MyExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception exception = context.Exception;
            logger.LogError(exception, "出现异常没处理");
            string message;
            if (env.IsDevelopment())
            {
                message = exception.ToString();
            }
            else
            {
                message = "程序出现异常未处理";
            }
            ObjectResult result = new ObjectResult(new { code = 500, message = message });
            result.StatusCode = 500;
            context.Result = result;
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
