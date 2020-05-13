using System;
namespace DependencyInjectionSample
{
    //HomeController利用了控制翻转设计原理，定义由HomeController使用的具体类的控件在外部给出
    public class HomeController
    {
        private readonly IGreetingService _greetingService;
        //构造函数注入
        public HomeController(IGreetingService greetingService)
        {
            //HomeController不依赖IGreetingService的具体实现
            _greetingService = greetingService ?? throw new ArgumentNullException(nameof(greetingService));
        }

        public string Hello(string name) => _greetingService.GreetWithOption(name);
    }
}
