using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Service;
using System.Net;
using System.Security.Claims;

namespace Usermanger.Filter
{
    public class JWTValidationFilter : IAsyncActionFilter
    {
        private readonly UserService userService;
        private readonly IMemoryCache cache;

        public JWTValidationFilter(IMemoryCache cache, UserService userService)
        {
            this.cache = cache;
            this.userService = userService;
    }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var value = context.HttpContext.User.FindFirst("id");
            if (value == null)
            {
                await next();
                return;
            }
            Guid id =new Guid(value.Value);
            string chacheKey = $"JWTVaildationFilter.UserInfo.{id}";
            Model.Entitys.User user = await cache.GetOrCreateAsync(chacheKey, async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return  userService.Find(id);
            });
            if (user == null)
            {
                var result = new ObjectResult($"UserId{id} not found");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result=result;
                return;
            }
            var claimVersion = context.HttpContext.User.FindFirst(ClaimTypes.Version);
            long jwtVerOfReq = long.Parse(claimVersion!.Value);
            if (jwtVerOfReq >= user.JWTVersion)
            {
                await next();
            }
            else
            {
                var result = new ObjectResult($"jwtversion mismatch，如：在其他地方登录");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                return;
            }
        }
    }
}
