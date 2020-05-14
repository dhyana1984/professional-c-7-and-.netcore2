using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class ParallelSample
    {
        static void PrintDiver(string content)
        {
            Console.WriteLine();
            Console.WriteLine($"----------{content}----------");
        }
        public static void DisplaySample()
        {

            //ParallelFor();
            //StopParallelForEarly();
            //ParallelForWithInit();
            //ParallelForEach();
            ParallelInvoke();
        }

        static void ParallelFor()
        {
            PrintDiver("ParallelFor");
            //前两个参数定义了循环开始和结束，表示从0迭代到9，即i>=0 && i<9
            //第3个参数是一个Action<int>委托，整数参数是循环迭代次数，传递给委托方法
            //返回是ParallelLoopResult， 提供了循环是否结束的消息
            ParallelLoopResult result = Parallel.For(0, 10, async i =>
            {
                Log($"S {i}");
                //Task.Delay(10).Wait();
                //Parallel.For不会等待延迟，只会等待它创建的任务，而不是其他后台活动
                await Task.Delay(10);
                Log($"E {i}");
            });
            Console.WriteLine($"Is completed: {result.IsCompleted}");
            Console.ReadLine();
        }

        //终止Parallel.For遍历
        static void StopParallelForEarly()
        {
            PrintDiver("StopParallelForEarly");
            //通过这个Parallel.For的重载，第三个委托参数有第二个参数ParallelLoopState，可以提前中断Parallel.For迭代
            ParallelLoopResult result = Parallel.For(10, 40, (int i, ParallelLoopState pls) =>
            {
                Log($"S {i}");
                if (i > 12)
                {
                    pls.Break();
                    Log($"break now... {i}");
                }
                Task.Delay(10).Wait();
                Log($"E {i}");
            });
            Console.WriteLine($"Is completed: {result.IsCompleted}");
            //LowestBreakIteration可以忽略其他不需要的任务结果
            Console.WriteLine($"lowest break iteration: {result.LowestBreakIteration}");
        }

        //Parallel.For<TLocal>方法可以对每个线程初始化
        static void ParallelForWithInit()
        {
            PrintDiver("ParallelForWithInit");
            //Parallel.For<TLocal>方法可以对每个线程初始化
            Parallel.For<string>(0, 10,
            //init方法
            //第一个委托参数是Func<TLocal>参数，返回一个TLocal类型返回值，本例是string
            //每个线程起始时执行一次
            () =>
            {
                Log($"init thread");
                return $"t{Thread.CurrentThread.ManagedThreadId}";
            },
            //第二个委托参数是Func<int, ParallelLoopState, string>，第一个int是循环迭代，第二个参数允许停止循环，循环体方法通过第三个参数从init方法接受返回的值
            //循环体定义了委托
            (i, pls, str1) =>
            {
                //对每个成员执行
                Log($"body i {i} str1 {str1}");
                Task.Delay(10).Wait();
                return $"i {i}";
            },
            //第三个委托参数是Action<TLocal>，接受从上一个委托中返回的值
            //对每个线程退出时调用一次
            (str1) =>
            {
                //每个线程退出时执行
                Log($"finally {str1}");
            }
            );
        }

        static void ParallelForEach()
        {
            PrintDiver("ParallelForEach");
            string[] data = { "zero", "one", "two", "three", "three", "four", "five", "six", "seven", "eight", "ten", "eleven", "twelve" };
            //data是集合，s是集合的元素
            ParallelLoopResult result = Parallel.ForEach<string>(data, s =>
            {
                Console.WriteLine(s);
            });

            //中断循环，通Parallel.For一样，传入第二个委托参数中的参数加上ParallelLoopState
            //第三个参数可以访问集合的索引器
            Parallel.ForEach<string>(data, (s, pls, l) =>
            {
                Console.WriteLine($"{s} {l}");
            });
        }

        //多个任务并行
        static void ParallelInvoke()
        {
            //并行任务用Parallel.Invoke
            //允许传入一个Action委托的数组
            Parallel.Invoke(Foo, Bar);

            void Foo() => Console.WriteLine("Foo");
            void Bar() => Console.WriteLine("Bar");
        }

        static void Log(string prefix) =>
           Console.WriteLine($"{prefix} task: {Task.CurrentId}, thread: {Thread.CurrentThread.ManagedThreadId}");
    }
}
