using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetanitExample
{
    public class PersonMiddleware
    {
        private readonly RequestDelegate next;
        public Person Person { get; }

        public PersonMiddleware(RequestDelegate next, IOptions<Person> options)
        {
            this.next = next;
            Person = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<p>Name: {Person?.Name}</p>");
            stringBuilder.Append($"<p>Age: {Person?.Age}</p>");
            stringBuilder.Append($"<p>Company: {Person?.Company?.Title}</p>");
            stringBuilder.Append("<h3>Languages</h3><ul>");
            foreach (string lang in Person.Languages)
                stringBuilder.Append($"<li>{lang}</li>");
            stringBuilder.Append("</ul>");

            await httpContext.Response.WriteAsync(stringBuilder.ToString());
        }
    }
}
