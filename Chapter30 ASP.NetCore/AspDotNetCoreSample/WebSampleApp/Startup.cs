using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSampleApp.Controllers;

namespace WebSampleApp
{
    public class Startup
    {
        //在依赖注入容器中配置服务
        //IServiceCollection包含Main方法中已注册的所有服务，并允许添加其他服务
        //因为派生于IList<T>，使用ServiceDescriptor作为泛型参数，所以可以改变和添加服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISampleService, DefaultSampleService>();
            //注册HomeController
            services.AddTransient<HomeController>();
        }

        //通过依赖注入接收参数
        //IWebHostEnvironment允许访问环境的名称，内容的根路径(源代码目录)，web的根路径(wwwroot)，默认提供程序是PhysicalFileProvider
        //IApplicationBuilder接口用于向HTTP请求管道添加中间件，调用Use方法时可以构建HTTP请求管，来响应请求时应该做什么
        //Use方法使用FluentAPI，返回IApplicationBuilder，可以很容易将多个中间件对象添加到管道中
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //检测是否Development环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/Home", homeApp =>
            {
                homeApp.Run(async context =>
                {
                    //homeApp.ApplicationServices返回IServiceProvider注入HomeController的实现
                    HomeController controller = homeApp.ApplicationServices.GetService<HomeController>();
                    await controller.Index(context);
                });
            });

            //Map实现路由
            app.Map("/RequestAndResponse", app1 =>
            {
                app1.Run(async context =>
                {
                    //响应返回请求的信息
                    //await context.Response.WriteAsync(RequestAndResponseSamples.GetRequestInformation(context.Request));
                    context.Response.ContentType = "text/html";
                    string result = string.Empty;

                    //根据请求的地址来调用不同的方法
                    switch (context.Request.Path.Value.ToLower())
                    {
                        case "/header":
                            result = RequestAndResponseSamples.GetHeaderInformation(context.Request);
                            break;
                        case "/add":
                            result = RequestAndResponseSamples.QueryString(context.Request);
                            break;
                        case "/content":
                            result = RequestAndResponseSamples.Content(context.Request);
                            break;
                        case "/encoded":
                            result = RequestAndResponseSamples.ContentEncoded(context.Request);
                            break;
                        case "/form":
                            result = RequestAndResponseSamples.GetForm(context.Request);
                            break;
                        case "/writecookie":
                            result = RequestAndResponseSamples.WriteCookie(context.Response);
                            break;
                        case "/readcookie":
                            result = RequestAndResponseSamples.ReadCookie(context.Request);
                            break;
                        case "/json":
                            result = RequestAndResponseSamples.GetJson(context.Response);
                            break;
                        default:
                            result = RequestAndResponseSamples.GetRequestInformation(context.Request);
                            break;
                    }

                    await context.Response.WriteAsync(result);
                });
            });

            //允许客户端请求wwwroot中的静态文件
            app.UseStaticFiles();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});

            //Run()方法在请求管道中注册最后一个中间件，方法参数是RequestDelegate类型的委托，接受HttpContext参数，返回一个Task
            //使用HttpContext可以访问来自浏览器的请求，并可以发送响应
            //app.Run(async (context) =>
            //{
            //    string[] lines = new[]
            //    {
            //        @"<ul>",
            //          @"<li><a href=""/hello.html"">Static Files</a> - requires UseStaticFiles</li>",
            //          @"<li>Request and Response",
            //            @"<ul>",
            //              @"<li><a href=""/RequestAndResponse"">Request and Response</a></li>",
            //              @"<li><a href=""/RequestAndResponse/header"">Header</a></li>",
            //              @"<li><a href=""/RequestAndResponse/add?x=38&y=4"">Add</a></li>",
            //              @"<li><a href=""/RequestAndResponse/content?data=sample"">Content</a></li>",
            //              @"<li><a href=""/RequestAndResponse/content?data=<h1>Heading 1</h1>"">HTML Content</a></li>",
            //              @"<li><a href=""/RequestAndResponse/content?data=<script>alert('hacker');</script>"">Bad Content</a></li>",
            //              @"<li><a href=""/RequestAndResponse/encoded?data=<h1>sample</h1>"">Encoded content</a></li>",
            //              @"<li><a href=""/RequestAndResponse/encoded?data=<script>alert('hacker');</script>"">Encoded bad Content</a></li>",
            //              @"<li><a href=""/RequestAndResponse/form"">Form</a></li>",
            //              @"<li><a href=""/RequestAndResponse/writecookie"">Write cookie</a></li>",
            //              @"<li><a href=""/RequestAndResponse/readcookie"">Read cookie</a></li>",
            //              @"<li><a href=""/RequestAndResponse/json"">JSON</a></li>",
            //            @"</ul>",
            //          @"</li>",
            //          @"<li><a href=""/Home"">Home Controller with dependency injection</a></li>",
            //          @"<li><a href=""/abc/xyz/42hello42/foobar"">MapWhen with hello in the URL</a></li>",
            //          @"<li><a href=""/Session"">Session</a></li>",
            //          @"<li>Configuration",
            //            @"<ul>",
            //              @"<li><a href=""/Configuration/appsettings"">Appsettings</a></li>",
            //              @"<li><a href=""/Configuration/colons"">Using Colons</a></li>",
            //              @"<li><a href=""/Configuration/database"">Database</a></li>",
            //              @"<li><a href=""/Configuration/stronglytyped"">Strongly Typed</a></li>",
            //            @"</ul>",
            //          @"</li>",
            //        @"</ul>"
            //    };

            //    var sb = new StringBuilder();
            //    foreach (var line in lines)
            //    {
            //        sb.Append(line);
            //    }
            //    string html = sb.ToString().HtmlDocument("Web Sample App");

            //    await context.Response.WriteAsync(html);
            //});
        }
    }
}
