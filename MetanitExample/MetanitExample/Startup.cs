using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace MetanitExample
{
    public class Startup
    {
        private static void Index(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Index");
            });
        }

        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("About");
            });
        }

        private static void HandleId(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("id is equal to 5");
            });
        }

        public Task SendResponseAsync(IDictionary<string, object> environment)
        {
            var requestHeaders = (IDictionary<string, string[]>)environment["owin.RequestHeaders"];
            string responseText = requestHeaders["User-Agent"][0];
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);

            var responseStream = (Stream)environment["owin.ResponseBody"];
            return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //
            //int x = 2;
            //app.Use(async (context, next) =>
            //{
            //    x = x * 2;      // 2 * 2 = 4
            //    await next.Invoke();    // вызов app.Run
            //    x = x * 2;      // 8 * 2 = 16
            //    await context.Response.WriteAsync($"Result: {x}");
            //});

            //app.Run(async (context) =>
            //{
            //    x = x * 2;  //  4 * 2 = 8
            //    await Task.FromResult(0);
            //});
            //

            //
            //app.Map("/home", home =>
            //{
            //    home.Map("/index", Index);
            //    home.Map("/about", About);
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Page Not Found");
            //});
            //

            //app.mapwhen(context =>
            //{
            //    return context.request.query.containskey("id") &&
            //            context.request.query["id"] == "5";
            //}, handleid);

            //app.run(async (context) =>
            //{
            //    await context.response.writeasync("good bye, world...");
            //});

            //app.UseMiddleware<TokenMiddleware>();
            //app.UseToken("12");
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World");
            //});

            //app.UseMiddleware<ErrorHandlingMiddleware>();
            //app.UseMiddleware<AuthenticationMiddleware>();
            //app.UseMiddleware<RoutingMiddleware>();

            //app.UseDefaultFiles();
            //app.UseDirectoryBrowser();

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
            //    RequestPath = new PathString("/pages")
            //});

            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
            //    RequestPath = new PathString("/pages")
            //});

            //app.UseFileServer(new FileServerOptions
            //{
            //    EnableDirectoryBrowsing = true,
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
            //    RequestPath = new PathString("/pages"),
            //    EnableDefaultFiles = true
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World");
            //});

            //app.UseOwin(pipeline =>
            //{
            //    pipeline(next => SendResponseAsync);
            //});

            //loggerFactory.AddConsole(LogLevel.Trace);

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");
            app.Run(async context =>
            {
                logger.LogInformation("Processing request {0}", context.Request.Path);

                await context.Response.WriteAsync("Hello Work!");
            });
        }
    }
}
