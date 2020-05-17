using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskAndParallelPrograming
{
    public static class DataFlow
    {
        public static void DisplaySample()
        {
            Task t1 = Task.Run(() => Producer());
            Task t2 = Task.Run(async () => await ConsumerAsync());
            Task.WaitAll(t1, t2);
        }
        //s_buffer既是数据源又是目标
        private static BufferBlock<string> s_buffer = new BufferBlock<string>();
        static async Task ConsumerAsync()
        {
            while (true)
            {
                string data = await s_buffer.ReceiveAsync();
                Console.WriteLine($"user input: {data}");
            }
        }

        static void Producer()
        {
            bool exit = false;
            while (!exit)
            {
                string input = Console.ReadLine();
                if (string.Compare(input, "exit", ignoreCase: true) == 0)
                {
                    exit = true;
                }
                else
                {
                    s_buffer.Post(input);
                }
            }
        }
    }
}
