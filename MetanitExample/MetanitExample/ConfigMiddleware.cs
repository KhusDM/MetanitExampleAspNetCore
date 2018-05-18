using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample
{
    public class ConfigMiddleware
    {
        private readonly RequestDelegate next;
        public IConfiguration AppConfiguration { get; set; }

        public ConfigMiddleware(RequestDelegate next, IConfiguration config)
        {
            this.next = next;
            AppConfiguration = config;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var color = AppConfiguration["color"];
            var text = AppConfiguration["text"];
            await httpContext.Response.WriteAsync($"<p style='color:{color};'>{text}</p>");
        }
    }
}
