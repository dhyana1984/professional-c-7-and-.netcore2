using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebSampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            BuildWebHost(args).Run();
        }

        //IHostBuilder是通用主机
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => //通过通用主机创建WebBuilder
                {
                    webBuilder.UseStartup<Startup>();
                });

        //直接使用IWebHostBuilder
        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>() //将执行StartUp的ConfigureServices和Configure方法
            .Build();
    }
}
