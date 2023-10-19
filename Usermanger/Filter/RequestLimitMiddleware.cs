using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Usermanger.Filter
{
    public class RequestLimitMiddleware
    {
        /*private readonly IMemoryCache memoryCache;

        public ActionMiddleware(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string remveIp=context.Connection.RemoteIpAddress!.ToString();
            //string remveIp = context.HttpContext.Connection.RemoteIpAddress!.ToString();
            string key = $"LastVisitTick_{remveIp}";
            long? lastTick = memoryCache.Get<long?>(key);
            if (lastTick == null || Environment.TickCount64 - lastTick > 100)
            {
                memoryCache.Set(key, Environment.TickCount64, TimeSpan.FromSeconds(1));
                return next(context);
            }
            else
            {
                var result = new ObjectResult("频繁点击");
                //result.StatusCode = 429;
                //context.Response.StatusCode = 200;
                //.Response.ContentType = "application/json";
                //context.Response.Body = 
                //return;
                // context.Result = new ContentResult { StatusCode = 429 };
                context.Response.ContentType = "text/plain";

                // Get a reference to the response stream
                var responseStream = context.Response.Body;

                // Write your data to the response stream
                byte[] data = Encoding.UTF8.GetBytes("恶意点击");
                responseStream.WriteAsync(data, 0, data.Length);

                // Don't forget to call the next middleware in the pipeline
                //return next(context);

                return Task.CompletedTask;
            }
        }*/
        private readonly RequestDelegate _next;
        private int _requestCount = 0;
        private const int MaxRequestCount = 100; // 限制每轮最多处理 100 个请求

        public RequestLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_requestCount < MaxRequestCount)
            {
                _requestCount++;
                await _next(context);
            }
            else
            {
                context.Response.WriteAsync("Too Many Requests Please slow down");
                context.Response.StatusCode = 429; // Too Many Requests
                
            }
        }
    }
}
