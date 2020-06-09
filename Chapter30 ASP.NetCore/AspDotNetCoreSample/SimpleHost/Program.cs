using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //context从客户端读取请求并返回内容
            WebHost.Start(async context =>
            {
                await context.Response.WriteAsync("<h1>A Simple Host!</h1>");
            }).WaitForShutdown();
        }
    }
}
