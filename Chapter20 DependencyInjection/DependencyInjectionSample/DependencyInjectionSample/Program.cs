using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DependencyInjectionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //通过构造函数注入
            //var controller = new HomeController(new GreetingService());
            //var result = controller.Hello("Matt");
            //Console.WriteLine(result);
            //Console.WriteLine();

            //指定配置
            DefineConfiguration();
            using (ServiceProvider container = RegisterServices())
            {
                //通过调用ServiceProvider对象的GetRequiredService<>()方法来获得注册的实例
                var controller = container.GetRequiredService<HomeController>();
                var result = controller.Hello("Matt");
                Console.WriteLine(result);
            }
            Console.WriteLine();
            SingletonAndTransient();
            Console.WriteLine();
            UsingScoped();
            Console.WriteLine();


        }

        static ServiceProvider RegisterServices()
        {
            //ServiceCollection的对象用来注册DI容器需要知道的类型
            var services = new ServiceCollection();
            ////AddSingleton添加单例
            //services.AddSingleton<IGreetingService, GreetingService>();
            //services.AddTransient<HomeController>();
            ////返回一个ServiceProvider对象，用来访问已注册的服务，即容器
            //return services.BuildServiceProvider();

            //AddGreetingService是对ServiceCollection的一个扩展方法，实际上执行的是
            //collection.Configure(setupAction);
            //return collection.AddTransient<IGreetingService, GreetingService>();
            //setupAction委托的是一个传入options的方法
            //services.AddGreetingService(options => options.From = "Christina" );
            services.AddOptions();
            //使用通过配置文件的方式传递参数，参数在指定到Configuration的json文件中的GreetingService内
            services.AddGreetingServiceWithConfig(Configuration.GetSection("GreetingService"));
            services.AddTransient<HomeController>();
            return services.BuildServiceProvider();
        }

        private static void SingletonAndTransient()
        {
            Console.WriteLine(nameof(SingletonAndTransient));

            //RegisterService是本地函数注册了服务
            ServiceProvider RegisterService()
            {
                IServiceCollection services = new ServiceCollection();
                //单例
                services.AddSingleton<IServiceA, ServiceA>();
                //临时服务
                services.AddTransient<IServiceB, ServiceB>();
                //临时服务
                //services.AddTransient<ControllerX>();
                //单例
                services.AddSingleton<INumberService, NumberService>();
                //直接使用Add方法注册
                //ServiceDescriptor用来构造服务类型，服务实现和服务种类，ServiceLifetime是服务周期的枚举
                services.Add(new ServiceDescriptor(typeof(ControllerX), typeof(ControllerX), ServiceLifetime.Transient));
                return services.BuildServiceProvider();
            }

            using (ServiceProvider container = RegisterService())
            {
                ControllerX x = container.GetRequiredService<ControllerX>();
                x.M();
                x.M();
                Console.WriteLine($"requesting {nameof(ControllerX)}");
                ControllerX x2 = container.GetRequiredService<ControllerX>();
            }
            Console.WriteLine();
        }

        private static void UsingScoped()
        {
            Console.WriteLine(nameof(UsingScoped));

            ServiceProvider RegisterServices()
            {
                var services = new ServiceCollection();
                services.AddSingleton<INumberService, NumberService>();
                services.AddScoped<IServiceA, ServiceA>();
                services.AddSingleton<IServiceB, ServiceB>();
                services.AddTransient<IServiceC, ServiceC>();
                return services.BuildServiceProvider();
            }

            using (ServiceProvider container = RegisterServices())
            {
                //调用ServiceProvider的CreateScope()方法创建一个作用域
                //返回实现接口IServiceScope的作用域对象，在其中可以访问属于这个作用域的ServiceProvider
                using (IServiceScope scope1 = container.CreateScope())
                {
                    //因为IServiceA注册为Scoped，所以在同一作用域下返回的实例时一样的
                    IServiceA a1 = scope1.ServiceProvider.GetService<IServiceA>();
                    a1.A(); //A, 1
                    IServiceA a2 = scope1.ServiceProvider.GetService<IServiceA>();
                    a2.A(); //A, 1
                    //scope末尾不会释放IServiceB因为IServiceB是注册为单例，需要在作用域的末尾也是存活的，在程序结束时才会释放
                    IServiceB b1 = scope1.ServiceProvider.GetService<IServiceB>();
                    b1.B();
                    IServiceC c1 = scope1.ServiceProvider.GetService<IServiceC>();
                    c1.C();
                    IServiceC c2 = scope1.ServiceProvider.GetService<IServiceC>();
                    c2.C();
                }

                Console.WriteLine("end of scope1");

                using (IServiceScope scope2 = container.CreateScope())
                {
                    IServiceA a3 = scope2.ServiceProvider.GetService<IServiceA>();
                    a3.A();
                    IServiceB b2 = scope2.ServiceProvider.GetService<IServiceB>();
                    b2.B();
                    IServiceC c3 = scope2.ServiceProvider.GetService<IServiceC>();
                    c3.C();
                }
                Console.WriteLine("end of scope2");
            }
            Console.WriteLine();
        }

        private static void CustomerFactories()
        {
            Console.WriteLine(nameof(CustomerFactories));

            IServiceB CreateServiceBFactory(IServiceProvider provider) =>
                new ServiceB(provider.GetService<INumberService>());

            ServiceProvider RegisterServices()
            {
                var numberService = new NumberService();
                var services = new ServiceCollection();
                //使用AddSingleton的重载版本，把一个已存在的实例传递给容器
                //这种方式容器不会调用Dispose方法，需要手动调用
                services.AddSingleton<INumberService>(numberService);
                //不是从容器创建，而是使用工厂方法创建
                //如果需要自定义的初始化或定义不受DI容器支持的构造函数，这是一个有用的选项
                //通过IServiceProvider参数传递委托，并返回服务实例给AddTransient
                services.AddTransient<IServiceB>(CreateServiceBFactory);
                services.AddSingleton<IServiceA, ServiceA>();
                return services.BuildServiceProvider();
            }

            using(ServiceProvider container = RegisterServices())
            {
                IServiceA a1 = container.GetService<IServiceA>();
                IServiceA a2 = container.GetService<IServiceA>();
                IServiceB b1 = container.GetService<IServiceB>();
                IServiceB b2 = container.GetService<IServiceB>();
            }
            Console.WriteLine();
        }

        public static IConfiguration Configuration { get; set; }
        static void DefineConfiguration()
        {
            //指定配置文件，并且读取到Configuration中
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
        }
    }
}
