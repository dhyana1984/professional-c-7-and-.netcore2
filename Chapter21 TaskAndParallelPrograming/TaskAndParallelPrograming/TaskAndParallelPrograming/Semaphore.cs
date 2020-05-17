using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class SemaphoreSample
    {
        public static void DisplaySample()
        {
            int taskCount = 6;
            int semaphoreCount = 3;
            //第一个参数是初始的信号量数量，第二个参数是最大的信号量数量
            var semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
            var tasks = new Task[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() => TaskMain(semaphore));
            }

            Task.WaitAll(tasks);

            Console.WriteLine("All tasks finished");
        }

        private static void TaskMain(SemaphoreSlim semaphore)
        {
            bool isCompleted = false;
            while (!isCompleted)
            {
                //通过Wait()方法锁定信号量
                if (semaphore.Wait(600))
                {
                    try
                    {
                        Console.WriteLine($"Task {Task.CurrentId} locks the semaphore");
                        Task.Delay(2000).Wait();
                    }
                    finally
                    {
                        Console.WriteLine($"Task {Task.CurrentId} releases the semaphore");
                        //释放信号量
                        semaphore.Release();
                        isCompleted = true;
                    }
                }
                else
                {
                    //如果没有分配到信号量，输出，并且重复循环申请信号量
                    Console.WriteLine($"Timeout for task {Task.CurrentId}; wait again");
                }
            }
        }
    }
}
