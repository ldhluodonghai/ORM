using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Usermanger.Filter
{
    public class RateActionFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache memoryCache;

        public RateActionFilter(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string remveIp = context.HttpContext.Connection.RemoteIpAddress!.ToString();
            string key = $"LastVisitTick_{remveIp}";
            long? lastTick = memoryCache.Get<long?>(key);
            if (lastTick == null || Environment.TickCount64 - lastTick > 1000)
            {
                memoryCache.Set(key, Environment.TickCount64,TimeSpan.FromSeconds(1));
                return next();
            }
            else
            {
                var result = new ObjectResult("点击频率过高");
                result.StatusCode = 429;
                context.Result = result;
                //return;
               // context.Result = new ContentResult { StatusCode = 429 };
                return Task.CompletedTask;
            }
        }
    }
}
