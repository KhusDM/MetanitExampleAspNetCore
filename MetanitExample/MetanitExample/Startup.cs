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
using Microsoft.Extensions.DependencyInjection;
using MetanitExample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;

namespace MetanitExample
{
    public class Startup
    {
        private IServiceCollection services;
        private IConfiguration AppConfiguration { get; set; }

        //public Startup(IHostingEnvironment env)
        //{
        //    //var builder = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        //    //{
        //    //    { "FirstName","Tom"},
        //    //    { "Age","31"}
        //    //});
        //    //var builder = new ConfigurationBuilder().SetBasePath(env.WebRootPath).AddJsonFile("conf.json");
        //    //var builder = new ConfigurationBuilder().SetBasePath(env.WebRootPath).AddXmlFile("conf.xml");
        //    //var builder = new ConfigurationBuilder().SetBasePath(env.WebRootPath).AddJsonFile("db.json");
        //    //var builder = new ConfigurationBuilder().SetBasePath(env.WebRootPath).AddJsonFile("content.json");
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddTextFile("config.txt");

        //    AppConfiguration = builder.Build();
        //}

        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;

            //var builder = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "color", "red" }, { "text", "Hello ASP.NET Core 2.0" } });

            //AppConfiguration = builder.Build();
        }

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

        private string GetSectionContent(IConfiguration configSection)
        {
            string sectionContent = "";
            foreach (var section in configSection.GetChildren())
            {
                sectionContent += "\"" + section.Key + "\":";
                if (section.Value == null)
                {
                    string subSectionContent = GetSectionContent(section);
                    sectionContent += "{\n" + subSectionContent + "},\n";
                }
                else
                {
                    sectionContent += "\"" + section.Value + "\",\n";
                }
            }
            return sectionContent;
        }

        private async Task Handle(HttpContext httpContext)
        {
            var routerValues = httpContext.GetRouteData().Values;
            var action = routerValues["action"].ToString();
            var name = routerValues["name"].ToString();
            var year = routerValues["year"].ToString();
            await httpContext.Response.WriteAsync($"action: {action} | name: {name} | year:{year}");
        }

        private async Task HandleInline(HttpContext httpContext)
        {
            var routeValues = httpContext.GetRouteData().Values;
            var controller = routeValues["controller"].ToString();
            var action = routeValues["action"].ToString();
            var id = routeValues["id"]?.ToString();

            await httpContext.Response.WriteAsync($"controller: {controller} | action: {action} | id: {id}");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDirectoryBrowser();
            //services.AddSession(options =>
            //{
            //    options.CookieName = ".MyApp.Session";
            //    options.IdleTimeout = TimeSpan.FromSeconds(3600);
            //});
            //services.AddDistributedMemoryCache();
            //services.AddTransient<IMessageSender, EmailMessageSender>();
            //services.AddTimeService();
            //services.AddTransient<MessageService>();
            //services.AddTransient<ICounter, RandomCounter>();
            //services.AddTransient<CounterService>();
            //services.AddScoped<ICounter, RandomCounter>();
            //services.AddScoped<CounterService>();
            //services.AddSingleton<ICounter, RandomCounter>();
            //services.AddSingleton<CounterService>();

            //RandomCounter randomCounter = new RandomCounter();
            //services.AddSingleton<ICounter>(randomCounter);
            //services.AddSingleton<CounterService>(new CounterService(randomCounter));
            //services.AddTransient<RandomCounter>();
            //services.AddTransient<ICounter>(provider =>
            //{
            //    var counter = provider.GetService<RandomCounter>();
            //    return counter;
            //});
            //services.AddTransient<CounterService>();
            //services.AddTransient<IMessageSender>(provider =>
            //{
            //    if (DateTime.Now.Hour >= 12) return new EmailMessageSender();
            //    else return new SmsMessageSender();
            //});
            //services.AddTransient<TimeService>();
            //services.AddTransient<IConfiguration>(provider => AppConfiguration);
            //services.AddOptions();
            //services.Configure<Person>(AppConfiguration);
            //services.Configure<Person>(option => { option.Age = 22; });

            //this.services = services;

            services.Configure<RouteOptions>(options => options.ConstraintMap.Add("position", typeof(PositionConstraint)));

            services.AddRouting();
        }

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, MessageService messageService, TimeService timeService, IMessageSender messageSender, ILoggerFactory loggerFactory)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    //
        //    //int x = 2;
        //    //app.Use(async (context, next) =>
        //    //{
        //    //    x = x * 2;      // 2 * 2 = 4
        //    //    await next.Invoke();    // вызов app.Run
        //    //    x = x * 2;      // 8 * 2 = 16
        //    //    await context.Response.WriteAsync($"Result: {x}");
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    x = x * 2;  //  4 * 2 = 8
        //    //    await Task.FromResult(0);
        //    //});
        //    //

        //    //
        //    //app.Map("/home", home =>
        //    //{
        //    //    home.Map("/index", Index);
        //    //    home.Map("/about", About);
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Page Not Found");
        //    //});
        //    //

        //    //app.mapwhen(context =>
        //    //{
        //    //    return context.request.query.containskey("id") &&
        //    //            context.request.query["id"] == "5";
        //    //}, handleid);

        //    //app.run(async (context) =>
        //    //{
        //    //    await context.response.writeasync("good bye, world...");
        //    //});

        //    //app.UseMiddleware<TokenMiddleware>();
        //    //app.UseToken("12");
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Hello World");
        //    //});

        //    //app.UseMiddleware<ErrorHandlingMiddleware>();
        //    //app.UseMiddleware<AuthenticationMiddleware>();
        //    //app.UseMiddleware<RoutingMiddleware>();

        //    //app.UseDefaultFiles();
        //    //app.UseDirectoryBrowser();

        //    //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
        //    //{
        //    //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
        //    //    RequestPath = new PathString("/pages")
        //    //});

        //    //app.UseStaticFiles();
        //    //app.UseStaticFiles(new StaticFileOptions()
        //    //{
        //    //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
        //    //    RequestPath = new PathString("/pages")
        //    //});

        //    //app.UseFileServer(new FileServerOptions
        //    //{
        //    //    EnableDirectoryBrowsing = true,
        //    //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\html")),
        //    //    RequestPath = new PathString("/pages"),
        //    //    EnableDefaultFiles = true
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Hello World");
        //    //});

        //    //app.UseOwin(pipeline =>
        //    //{
        //    //    pipeline(next => SendResponseAsync);
        //    //});

        //    //loggerFactory.AddConsole(LogLevel.Trace);

        //    //loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
        //    //var logger = loggerFactory.CreateLogger("FileLogger");
        //    //app.Run(async context =>
        //    //{
        //    //    logger.LogInformation("Processing request {0}", context.Request.Path);

        //    //    await context.Response.WriteAsync("Hello Work!");
        //    //});

        //    //app.Use(async (context, next) =>
        //    //{
        //    //    context.Items["text"] = "Text from HttpContext.Items";
        //    //    await next.Invoke();
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    context.Response.ContentType = "text/html; charset=utf-8";
        //    //    await context.Response.WriteAsync(String.Format("Text: {0}", context.Items["text"]));
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    if (context.Request.Cookies.ContainsKey("name"))
        //    //    {
        //    //        string name = context.Request.Cookies["name"];
        //    //        await context.Response.WriteAsync(String.Format("Hellow {0}!", name));
        //    //    }
        //    //    else
        //    //    {
        //    //        context.Response.Cookies.Append("name", "Tom");
        //    //        await context.Response.WriteAsync(String.Format("Hello world!"));
        //    //    }
        //    //});

        //    //app.UseSession();
        //    //app.Run(async (context) =>
        //    //{
        //    //    if (context.Session.Keys.Contains("name"))
        //    //    {
        //    //        string name = context.Session.GetString("name");
        //    //        await context.Response.WriteAsync(String.Format("Hello {0}", name));
        //    //    }
        //    //    else
        //    //    {
        //    //        context.Session.SetString("name", "Tom");
        //    //        await context.Response.WriteAsync("Hello World!");
        //    //    }
        //    //});

        //    //app.UseSession();
        //    //app.Run(async (context) =>
        //    //{
        //    //    if (context.Session.Keys.Contains("person"))
        //    //    {
        //    //        Person person = context.Session.Get<Person>("person");
        //    //        await context.Response.WriteAsync(String.Format("Hello {0}", person.Name));
        //    //    }
        //    //    else
        //    //    {
        //    //        Person person = new Person() { Name = "IVan", Age = 22 };
        //    //        context.Session.Set<Person>("person", person);
        //    //        await context.Response.WriteAsync(String.Format("Hello World"));
        //    //    }
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    var sb = new StringBuilder();
        //    //    sb.Append("<h1>Все сервисы</h1>");
        //    //    sb.Append("<table>");
        //    //    sb.Append("<tr><th>Тип</th><th>Lifetime</th><th>Реализация</th></tr>");
        //    //    foreach (var svc in services)
        //    //    {
        //    //        sb.Append("<tr>");
        //    //        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
        //    //        sb.Append($"<td>{svc.Lifetime}</td>");
        //    //        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
        //    //        sb.Append("</tr>");
        //    //    }
        //    //    sb.Append("</table>");
        //    //    context.Response.ContentType = "text/html;charset=utf-8";
        //    //    await context.Response.WriteAsync(sb.ToString());
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync(messageSender.Send());
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    context.Response.ContentType = "text/html; charset=utf-8";
        //    //    await context.Response.WriteAsync($"Время :{timeService.GetTime()}");
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync(messageService.Send());
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    IMessageSender messageSender2 = context.RequestServices.GetService<IMessageSender>();
        //    //    context.Response.ContentType = "text/html;charset=utf-8";
        //    //    await context.Response.WriteAsync(messageSender2.Send());
        //    //});

        //    //app.Run(async (context) =>
        //    //{
        //    //    IMessageSender messageSender3 = app.ApplicationServices.GetService<IMessageSender>();
        //    //    context.Response.ContentType = "text/html;charset=utf-8";
        //    //    await context.Response.WriteAsync(messageSender3.Send());
        //    //});

        //    //app.UseMiddleware<MessageMiddleware>();

        //    //app.UseMiddleware<CounterMiddleware>();

        //    //app.UseMiddleware<TimerMiddleware>();
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Hello World!");
        //    //});

        //    //AppConfiguration["FirstName"] = "Alice";
        //    //AppConfiguration["LastName"] = "Simpson";
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync(AppConfiguration["FirstName"] + " " + AppConfiguration["LastName"]);
        //    //});

        //    //var color = AppConfiguration["color"];
        //    //var text = AppConfiguration["text"];
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync($"<p style='color:{color};'>{text}</p>");
        //    //});

        //    //IConfigurationSection connStrings = AppConfiguration.GetSection("ConnectionStrings");
        //    //string defaultConnection = connStrings.GetSection("DefaultConnection").Value;
        //    //string con = AppConfiguration["ConnectionStrings:DefaultConnection"];
        //    //app.Run(async (context) =>
        //    //{
        //    //    context.Response.ContentType = "text/html;charset=utf-8";
        //    //    await context.Response.WriteAsync(defaultConnection + " " + con);
        //    //});

        //    //string projectJsonContent = GetSectionContent(AppConfiguration);
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("{\n" + projectJsonContent + "}");
        //    //});

        //    //var color = AppConfiguration["color"];
        //    //var text = AppConfiguration["text"];
        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync($"<p style='color:{color};'>{text}</p>");
        //    //});

        //    //app.UseMiddleware<ConfigMiddleware>();
        //}

        public void Configure(IApplicationBuilder app)
        {
            //app.UseMiddleware<PersonMiddleware>();

            //var myRouteHandler = new RouteHandler(Handle);
            //var routeBuilder = new RouteBuilder(app, myRouteHandler);
            //routeBuilder.MapRoute("default", "{controller}/{action}");
            //app.UseRouter(routeBuilder.Build());

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //var routeBuilder = new RouteBuilder(app);

            //routeBuilder.MapRoute("{controller}/{action}", async context =>
            // {
            //     context.Response.ContentType = "text/html; charset=utf-8";
            //     await context.Response.WriteAsync("двухсегментный запрос");
            // });

            //routeBuilder.MapRoute("{controller}/{action}/{id}", async context =>
            // {
            //     context.Response.ContentType = "text/html; charset=utf-8";
            //     await context.Response.WriteAsync("трехсегментный запрос");
            // });

            //var myRouteHandler = new RouteHandler(Handle);
            //var routeBuilder = new RouteBuilder(app, myRouteHandler);
            //routeBuilder.MapRoute("default", "{action=Index}/{name}-{year}");
            //var routeBuilder = new RouteBuilder(app);
            //routeBuilder.MapRoute("{controller}/{action:alpha:minlength(6)}/{id?}",async context=> 
            //{
            //    await context.Response.WriteAsync("e");
            //});

            var myRouterHandler = new RouteHandler(HandleInline);
            var routeBuilder = new RouteBuilder(app, myRouterHandler);
            routeBuilder.MapRoute("default", "{controller}/{action}/{id:position?}");

            app.UseRouter(routeBuilder.Build());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
