using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class TaskSample
    {
       private static object s_logLock = new object();
       public static void DisplaySample()
        {
            //TasksUsingThreadPool();
            //RunSynchronousTask();
            //LongRunningTask();
            //TaskWithResultDemo();
            //ContinuationTasks();
            ParentAndChild();
        }

        static void TasksUsingThreadPool()
        {
            PrintDiver("TasksUsingThreadPool");
            //第一种创建任务方式，使用实例化的TaskFactory类
            //把TaskMethod方法传给StartNew方法
            var tf = new TaskFactory();
            Task t1 = tf.StartNew(TaskMethod, "using a task factory");
            //第二种创建任务方式，使用Task类的静态属性Factory来访问TaskFactory，对工厂控制没有那么全面
            //把TaskMethod方法传给StartNew方法
            Task t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");
            //第三种方式是使用Task类的构造函数，实例化Task对象时，任务不会立即运行，而是指定Created状态，接着调用Task类的Start方法
            var t3 = new Task(TaskMethod, "using a task constructor and Start");
            t3.Start();
            //第四种方式调用Task类的Run方法
            Task t4 = Task.Run(() => TaskMethod("using the Run method"));
        }

        //以相同的线程作为主调线程
        private static void RunSynchronousTask()
        {
            PrintDiver("RunSynchronousTask");
            //在主线程上调用TaskMethod
            TaskMethod("just the main thread");
            //然后在创建的Task上调用TaskMethod
            var t1 = new Task(TaskMethod, "run sync");
            //使用相同的线程作为主调用线程，但是如果之前没创建任务，会创建一个任务
            t1.RunSynchronously();
            //is pooled thread: False
            //is background thread: False
        }

        //使用单独的线程的任务
        private static void LongRunningTask()
        {
            PrintDiver("LongRunningTask");
            //如果是长时间运行的任务，需要在使用Task构造函数创建时，加上TaskCreationOptions.LongRunning参数
            //此时该任务不会从线程池中创建，任务调度器会立刻辨识改任务并且不会等待该任务
            var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
            t1.Start();
            //is pooled thread: False
            //is background thread: True
        }

        //返回结果的Task
        private static void TaskWithResultDemo()
        {
            PrintDiver("TaskWithResultDemo");
            //Task<TResult>定义了返回类型，第一个参数是方法委托，第二个参数是输入参数
            var t1 = new Task<(int Result, int Remainder)>(TaskWithResult, (8, 3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine($"result from task: {t1.Result.Result} {t1.Result.Remainder}");
        }

        //连续任务
        private static void ContinuationTasks()
        {
            PrintDiver("ContinuationTasks");
            Task t1 = new Task(DoOnFirst);
            Task t2 = t1.ContinueWith(DoOnSecond);
            Task t3 = t1.ContinueWith(DoOnSecond);
            Task t4 = t2.ContinueWith(DoOnSecond);

            //通过TaskContinuationOptions设置连续任务启动的时机
            //Task t5 = t1.ContinueWith(DoOnError, TaskContinuationOptions.OnlyOnFaulted);
            t1.Start();
            Console.ReadLine();
        }

        //任务层次结构
        public static void ParentAndChild()
        {
            PrintDiver("ParentAndChild");
            //在父任务中又会创建子任务
            var parent = new Task(ParentTask);
            parent.Start();
            Task.Delay(2000).Wait();
            Console.WriteLine(parent.Status);
            Task.Delay(4000).Wait();
            Console.WriteLine(parent.Status);
        }

        private static void ParentTask()
        {
            Console.WriteLine($"task id {Task.CurrentId}");
            var child = new Task(ChildTask);
            child.Start();
            Task.Delay(1000).Wait();
            Console.WriteLine("parent started child");
        }

        private static void ChildTask()
        {
            Console.WriteLine("child");
            Task.Delay(5000).Wait();
            Console.WriteLine("child finished");
        }

        private static void DoOnFirst()
        {
            Console.WriteLine($"doing some task {Task.CurrentId}");
            Task.Delay(3000).Wait();
        }

        private static void DoOnSecond(Task t)
        {
            Console.WriteLine($"task {t.Id} finished");
            Console.WriteLine($"this task id {Task.CurrentId}");
            Console.WriteLine("do some cleanup");
            Task.Delay(3000).Wait();
        }

        static (int Result, int Remainder) TaskWithResult(object division)
        {
            (int x, int y) = ((int x, int y))division;
            int result = x / y;
            int remainder = x % y;
            Console.WriteLine("task create a result...");
            return (result, remainder);
        }

        static void TaskMethod(Object o)
        {
            Log(o?.ToString());
        }

         static void Log(string title)
        {
            lock (s_logLock)
            {
                Console.WriteLine(title);
                Console.WriteLine($"Task id: {Task.CurrentId?.ToString() ?? "no task"}, thread: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"is pooled thread: {Thread.CurrentThread.IsThreadPoolThread}");
                Console.WriteLine($"is background thread: {Thread.CurrentThread.IsBackground}");
                Console.WriteLine();
            }
        }

        static void PrintDiver(string content)
        {
            Console.WriteLine();
            Console.WriteLine($"----------{content}----------");
        }
    }
}
