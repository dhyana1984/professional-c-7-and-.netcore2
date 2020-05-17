using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class ReaderWriterLockSlimSample
    {
        public static void DisplaySample()
        {
            var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning,
            TaskContinuationOptions.None);
            var tasks = new Task[6];
            tasks[0] = taskFactory.StartNew(WriterMethod, 1);
            tasks[1] = taskFactory.StartNew(ReaderMethod, 1);
            tasks[2] = taskFactory.StartNew(ReaderMethod, 2);
            tasks[3] = taskFactory.StartNew(WriterMethod, 2);
            tasks[4] = taskFactory.StartNew(ReaderMethod, 3);
            tasks[5] = taskFactory.StartNew(ReaderMethod, 4);

            Task.WaitAll(tasks);
        }
        private static List<int> _items = new List<int>() { 0, 1, 2, 3, 4, 5 };
        //ReaderWriterLockSlim允许锁定多个读取器，如果没有写入器锁定资源，允许多个读取器访问资源
        //但只能有一个写入器锁定资源
        private static ReaderWriterLockSlim _rwl =
          new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public static void ReaderMethod(object reader)
        {
            try
            {
                //读取锁定
                //EnterReadLock阻塞线程
                //读取器不必限制，如果写入器被占用，读取器不能访问资源，被阻塞在此
                _rwl.EnterReadLock();
                for (int i = 0; i < _items.Count; i++)
                {
                    Console.WriteLine($"reader {reader}, loop: {i}, item: {_items[i]}");
                    Task.Delay(40).Wait();
                }
            }
            finally
            {
                _rwl.ExitReadLock();
            }
        }

        public static void WriterMethod(object writer)
        {
            try
            {
                //写入锁定
                //TryEnterWriteLock是不阻塞线程的
                //在抢占到写入锁之前不能离开循环
                while (!_rwl.TryEnterWriteLock(50))
                {
                    Console.WriteLine($"Writer {writer} waiting for the write lock");
                    Console.WriteLine($"current reader count: {_rwl.CurrentReadCount}");
                }
                Console.WriteLine($"Writer {writer} acquired the lock");
                for (int i = 0; i < _items.Count; i++)
                {
                    _items[i]++;
                    Task.Delay(50).Wait();
                }
                Console.WriteLine($"Writer {writer} finished");
            }
            finally
            {
                _rwl.ExitWriteLock();
            }
        }
    }
}
