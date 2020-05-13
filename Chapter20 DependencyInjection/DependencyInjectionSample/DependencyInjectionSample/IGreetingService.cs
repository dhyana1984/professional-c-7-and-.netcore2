using System;
namespace DependencyInjectionSample
{
    public interface  IGreetingService
    {
        string Greet(string name);

        string GreetWithOption(string name);
        
    }

}
