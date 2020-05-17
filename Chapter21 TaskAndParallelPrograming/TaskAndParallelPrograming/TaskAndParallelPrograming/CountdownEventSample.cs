using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class CountdownEventSample
    {
        public static void DisplaySample()
        {

            const int taskCount = 4;
            //创建事件对象，初始化taskcount
            var cEvent = new CountdownEvent(taskCount);
            var calcs = new Calculator[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                calcs[i] = new Calculator(cEvent);
                int i1 = i;
                Task.Run(() => calcs[i1].Calculation(i1 + 1, i1 + 3));
            }
            //等待所有CountdownEvent对象都调用了Signal()达到计数
            cEvent.Wait();
            Console.WriteLine("all finished");

            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"task for {i}, result: {calcs[i].Result}");
            }
        }
    }

    public class Calculator
    {
        private CountdownEvent _cEvent;

        public int Result { get; private set; }

        public Calculator(CountdownEvent ev)
        {
            _cEvent = ev;
        }

        public void Calculation(int x, int y)
        {
            Console.WriteLine($"Task {Task.CurrentId} starts calculation");
            Task.Delay(new Random().Next(3000)).Wait();
            Result = x + y;

            // signal the event-completed!
            Console.WriteLine($"Task {Task.CurrentId} is ready");
            //调用CountdownEvent的Signal()_cEvent对象的计数+1
            _cEvent.Signal();
        }
    }

}
