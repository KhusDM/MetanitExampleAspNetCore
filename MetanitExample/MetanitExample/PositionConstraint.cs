using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample
{
    public class PositionConstraint : IRouteConstraint
    {
        string[] positions = { "admin", "director", "accountant" };

        public bool Match(HttpContext httpContext, IRouter router, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return positions.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }
}
