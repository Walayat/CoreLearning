using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.API.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timetoliveSeconds;

        public CachedAttribute(int timetoliveSeconds)
        {
            this.timetoliveSeconds = timetoliveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        }

        private string GenerateCacheKeyFromRequest(HttpRequest httpRequest)
        {
            var keyBuilder = new System.Text.StringBuilder();
            keyBuilder.Append($"{httpRequest.Path }");

            foreach (var (key,value) in httpRequest.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"|{key} - {value}");
            }
            return keyBuilder.ToString();
        }
    }
}
