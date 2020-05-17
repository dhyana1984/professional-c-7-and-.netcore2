using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public class LockAndAwait
    {
        public async static void DisplaySample()
        {
            await RunUseSemaphoreAsync();
            await RunUseAsyncSempahoreAsync();
            Console.ReadLine();
        }

        //private static object s_syncLock = new object();
        //static async Task IncorrectLockAsync()
        //{
        //    //在async方法中不能使用lock
        //    //因为async完成之后，该方法可能在一个不同的线程中运行，而不是在async之前的线程中运行,不能从线程1获取锁在线程2释放锁
        //    lock (s_syncLock)
        //    {
        //        Console.WriteLine($"{nameof(IncorrectLockAsync)} started");
        //        await Task.Delay(500);  // compiler error: cannot await in the body of a lock statement
        //        Console.WriteLine($"{nameof(IncorrectLockAsync)} ending");
        //    }
        //}

        static async Task RunUseSemaphoreAsync()
        {
            Console.WriteLine(nameof(RunUseSemaphoreAsync));
            string[] messages = { "one", "two", "three", "four", "five", "six" };
            Task[] tasks = new Task[messages.Length];

            for (int i = 0; i < messages.Length; i++)
            {
                string message = messages[i];

                tasks[i] = Task.Run(async () =>
                {
                    await LockWithSemaphore(message);
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine();
        }

        static async Task RunUseAsyncSempahoreAsync()
        {
            Console.WriteLine(nameof(RunUseAsyncSempahoreAsync));
            string[] messages = { "one", "two", "three", "four", "five", "six" };
            Task[] tasks = new Task[messages.Length];

            for (int i = 0; i < messages.Length; i++)
            {
                string message = messages[i];

                tasks[i] = Task.Run(async () =>
                {
                    await UseAsyncSemaphore(message);
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine();
        }

        //使用Semaphore或者SemaphoreSlim可以从不同的线程中释放信号量
        private static SemaphoreSlim s_asyncLock = new SemaphoreSlim(1);
        static async Task LockWithSemaphore(string title)
        {
            Console.WriteLine($"{title} waiting for lock");
            //占用信号量
            await s_asyncLock.WaitAsync();
            try
            {
                Console.WriteLine($"{title} {nameof(LockWithSemaphore)} started");
                await Task.Delay(500);
                Console.WriteLine($"{title} {nameof(LockWithSemaphore)} ending");
            }
            finally
            {
                //使用完之后释放
                s_asyncLock.Release();
            }
        }

        private static AsyncSemaphore s_asyncSemaphore = new AsyncSemaphore();
        static async Task UseAsyncSemaphore(string title)
        {
            //AsyncSemaphore的对象在IDisposeable.Dispose()方法中释放了，所以可以使用using
            using (await s_asyncSemaphore.WaitAsync())
            {
                Console.WriteLine($"{title} {nameof(LockWithSemaphore)} started");
                await Task.Delay(500);
                Console.WriteLine($"{title} {nameof(LockWithSemaphore)} ending");
            }
        }

    }

    public sealed class AsyncSemaphore
    {
        private class SemaphoreReleaser : IDisposable
        {
            private SemaphoreSlim _semaphore;

            public SemaphoreReleaser(SemaphoreSlim semaphore) =>
                _semaphore = semaphore;

            public void Dispose() => _semaphore.Release();
        }

        private SemaphoreSlim _semaphore;
        public AsyncSemaphore() =>
            _semaphore = new SemaphoreSlim(1);

        public async Task<IDisposable> WaitAsync()
        {
            await _semaphore.WaitAsync();
            return new SemaphoreReleaser(_semaphore) as IDisposable;
        }
    }
}
