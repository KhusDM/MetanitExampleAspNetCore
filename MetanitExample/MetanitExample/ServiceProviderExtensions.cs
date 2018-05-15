using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MetanitExample.Services;

namespace MetanitExample
{
    public static class ServiceProviderExtensions
    {
        public static void AddTimeService(this IServiceCollection services)
        {
            services.AddTransient<TimeService>();
        }
    }
}
