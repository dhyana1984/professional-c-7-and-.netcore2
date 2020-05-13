using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class ParallelSample
    {
        static void PrintDiver(string content)
        {
            Console.WriteLine($"----------{content}----------");
        }
        public static void DisplaySample()
        {
            PrintDiver("ParallelFor");
            ParallelFor();
        }

        static void ParallelFor()
        {
            ParallelLoopResult result = Parallel.For(0, 10, i =>
            {
                Log($"S {i}");
                Task.Delay(10).Wait();
                Log($"E {i}");
            });
            Console.WriteLine($"Is completed: {result.IsCompleted}");
        }

        static void Log(string prefix) =>
           Console.WriteLine($"{prefix} task: {Task.CurrentId}, thread: {Thread.CurrentThread.ManagedThreadId}");
    }
}
