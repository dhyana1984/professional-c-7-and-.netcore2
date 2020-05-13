using System;
using Microsoft.Extensions.Options;

namespace DependencyInjectionSample
{
    public class GreetingService : IGreetingService
    {
        private readonly string _from;

        //通过IOptions<GreetingServiceOptions>来定义注入时初始化GreetingService的设置
        //Form就是通过注入时初始化传入的信息
        public GreetingService(IOptions<GreetingServiceOptions> options) => _from = options.Value.From;
        
        public string Greet(string name) => $"Hello, {name}";
        public string GreetWithOption(string name) => $"Hello, {name}, Greeting from {_from}";

    }

   
}
