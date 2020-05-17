using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAndParallelPrograming
{
    public static class CancellationSample
    {
       


        public static void DisplaySample()
        {
            //CancelParallelFor();
            CancelTask();
        }

        static void CancelParallelFor()
        {
            PrintDiver("CancelParallelFor");
            //CancellationToken通过创建CancellationTokenSource来生成
            var cts = new CancellationTokenSource();
            //Register方法接受Action和ICancelableOperation类型的参数，Action在取消标记时调用，实现ICancelableOperation的Cancel在执行取消时调用
            //CancellationTokenSource实现了ICancelableOperation接口，可以用CancellationToken注册，并允许使用Cancel()方法取消操作
            //使用Register目的是注册取消操作时的信息
            cts.Token.Register(() => Console.WriteLine("*** token cancelled"));
            //CancelAfter在500毫秒后取消标记
            cts.CancelAfter(500);
            try
            {
                ParallelLoopResult result =
                    Parallel.For(0, 100,
                    //ParallelOptions可以传递一个CancellationToken参数
                    new ParallelOptions { CancellationToken = cts.Token },
                    x =>
                    {
                        Console.WriteLine($"loop {x} started");
                        int sum = 0;
                        for (int i = 0; i < 100; i++)
                        {
                            Task.Delay(2).Wait();
                            sum += i;
                        }
                        Console.WriteLine($"loop {x} finished");
                    });
            }
            //Parallel类验证CancellationToken的结果，并取消操作
            //一旦取消操作,For()就会抛出一个OperationCanceledException异常
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void CancelTask()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("*** task cancelled"));
            //500毫秒后取消
            cts.CancelAfter(600);
            Task t1 = Task.Run(
            //Task.Run第一个参数是Action
            () =>
            {
                Console.WriteLine("in Task");
                for (int i = 0; i < 20; i++)
                {
                    Task.Delay(100).Wait();
                    CancellationToken token = cts.Token;
                    //检查取消标记
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("cancelling was request, calcelling from within the task");
                        //ThrowIfCancellationRequested会抛出TaskCanceledException异常，可以用于任务中运行Parallel.For()取消层次结构
                        token.ThrowIfCancellationRequested();
                        break;
                    }
                    Console.WriteLine("in loop");
                }
                Console.WriteLine("task finished without cancellation");
                //第二个参数是CancellationToken
            }, cts.Token);

            try
            {
                t1.Wait();
            }
            catch(AggregateException ex)
            {
                Console.WriteLine( $"exception: {ex.GetType().Name}, {ex.Message}");
                foreach (var item in ex.InnerExceptions)
                {
                    Console.WriteLine($"inner exceptions: {ex.InnerException.GetType()}, {ex.InnerException.Message}");
                }
            }
        }

        static void PrintDiver(string content)
        {
            Console.WriteLine();
            Console.WriteLine($"----------{content}----------");
        }
    }
   
}
