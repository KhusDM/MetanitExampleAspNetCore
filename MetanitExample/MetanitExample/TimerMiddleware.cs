using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MetanitExample.Services;

namespace MetanitExample
{
    public class TimerMiddleware
    {
        private readonly RequestDelegate next;
        TimeService timeService;

        public TimerMiddleware(RequestDelegate next, TimeService timeService)
        {
            this.next = next;
            this.timeService = timeService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.ToLower() == "/time")
            {
                httpContext.Response.ContentType = "text/html; charset=utf-8";
                await httpContext.Response.WriteAsync($"Текущее время: {timeService.GetTime()}");
            }
            else
            {
                await next.Invoke(httpContext);
            }
        }
    }
}
