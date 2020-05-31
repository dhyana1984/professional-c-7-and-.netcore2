using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UsingDependencyInjection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            InitializeServices();
            var service = Container.GetService<BooksService>();
            //await service.AddBooksAsync();
            await service.ReadBooksAsync();
        }

        private static ServiceProvider Container { get;  set; }

        //初始化依赖注入框架的容器
        private static void InitializeServices()
        {
            const string ConnectionString =
              @"server=192.168.1.7\SQLEXPRESS; User Id=sa;Password=xiongyi1984; database=Books;";
            var services = new ServiceCollection();
            //向ServiceCollection中添加BooksService类
            //每次请求这个服务就实例化ServiceCollection
            services.AddTransient<BooksService>()
                //EF Core 3.0+ 不再需要AddEntityFrameworkSqlServer
                //.AddEntityFrameworkSqlServer()
                //AddDbContext接受一个委托参数，来接受DbContextOptionsBuilder参数
                .AddDbContext<BooksContext>(options =>
                    options.UseLoggerFactory(MyLoggerFactory).UseSqlServer(ConnectionString)
                );
            services.AddLogging();
            Container = services.BuildServiceProvider();
        }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
