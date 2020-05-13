using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionSample
{
    public static class GreetingServieExtentions
    {
       public static IServiceCollection AddGreetingService(this IServiceCollection collection, Action<GreetingServiceOptions> setupAction)
        {
            if(collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if(setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            //Configure方法用于通过IOptions接口指定配置
            //将委托方法中的From传递给GreetingServiceOptions
            collection.Configure(setupAction);
            //Action<GreetingServiceOptions>会自动匹配到初始化GreetingService的参数上
            //因为在GreetingService的构造函数是GreetingService(IOptions<GreetingServiceOptions> options)
            return collection.AddTransient<IGreetingService, GreetingService>();
        }

        public static IServiceCollection AddGreetingServiceWithConfig
            (
                this IServiceCollection collection,
                //IConfiguration可以接收配置在文件中的值
                IConfiguration config
            )
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            //config用将其传递给Configure方法
            //将Json文件中的From属性传递给GreetingServiceOptions
            collection.Configure<GreetingServiceOptions>(config);
            return collection.AddTransient<IGreetingService, GreetingService>();
        }
    }
}
