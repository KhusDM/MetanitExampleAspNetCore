using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace MetanitExample
{
    public static class TokenExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern)
        {
            return builder.UseMiddleware<TokenMiddleware>(pattern);
        }
    }
}
