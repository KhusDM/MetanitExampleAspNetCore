using MetanitExample.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample
{
    public class CounterMiddleware
    {
        private readonly RequestDelegate next;
        private int i = 0;

        public CounterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, ICounter counter, CounterService counterService)
        {
            i++;
            httpContext.Response.ContentType = "text/html;charset=utf-8";
            await httpContext.Response.WriteAsync($"Запрос {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
        }
    }
}
