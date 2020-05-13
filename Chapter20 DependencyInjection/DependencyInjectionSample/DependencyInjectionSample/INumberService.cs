using System;
using System.Threading;

namespace DependencyInjectionSample
{
    public interface INumberService
    {
        int GetNumber();
    }

    public class NumberService : INumberService
    {
        private int _number = 0;
        //Interlocked.Increment是原子操作，相当于lock(object){ i++;}
        public int GetNumber() => Interlocked.Increment(ref _number);
        
    }
}
