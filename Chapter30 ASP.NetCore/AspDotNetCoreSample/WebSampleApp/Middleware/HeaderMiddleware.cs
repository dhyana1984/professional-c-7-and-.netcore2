using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebSampleApp.Middleware
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;
        //RequestDelegate委托接受HttpContext作为参数，并返回一个Task
        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Invoke匹配RequestDelegate委托
        public Task Invoke(HttpContext httpContext)
        {
            //在header里面加内容
            //在此可以访问请求和响应信息
            httpContext.Response.Headers.Add("sampleheader", new[] { "addheadermiddleware" });
            //调用下一个中间件模板
            return _next(httpContext);
        }
    }

    //扩展IApplicationBuilder，使用UseHeaderMiddleware来调用HeaderMiddleware
    public static class HeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<HeaderMiddleware>();
    }
}
