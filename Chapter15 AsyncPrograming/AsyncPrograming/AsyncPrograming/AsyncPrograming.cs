using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPrograming
{
    public static class AsyncPrograming
    {
        public static void TraceThreadAndTask(string info)
        {
            string taskInfo = Task.CurrentId == null ? "no task" : "task" + Task.CurrentId;
            Console.WriteLine($"{info} in thread {Thread.CurrentThread.ManagedThreadId}" + $" and {taskInfo}");
        }

        public static string Greeting(string name)
        {
            TraceThreadAndTask($"running {nameof(Greeting)}");
            Task.Delay(3000).Wait();
            return $"Hello, {name}";
        }

        public static Task<string> GreetingAsync(string name) =>
            //Task.Run方法返回一个任务
            Task.Run<string>(() => //返回Task类型需要使用Task.Run
            {
                TraceThreadAndTask($"running {nameof(GreetingAsync)}");
                //在Task.Run的委托参数中调用需要执行的同步方法
                return Greeting(name); 
            }); 

        //调用异步方法
        public async static void CallerWithAsync()
        {
            TraceThreadAndTask($"started {nameof(CallerWithAsync)}");
            //这里不会等待GreetingAsync()执行完毕，主线程会直接往下走
            Console.WriteLine(await GreetingAsync("Tom"));
            TraceThreadAndTask($"ended {nameof(CallerWithAsync)}");
        }

        //使用Awaiter
        public static void CallerWithAwaiter()
        {
            TraceThreadAndTask($"started {nameof(CallerWithAwaiter)}");
            //GetAwaiter返回一个TaskAwaiter
            //可以对任何提供GetAwaiter方法的对象使用async关键字
            TaskAwaiter<string> awaiter = GreetingAsync("Jacl").GetAwaiter();
            //使用OnCompletet方法实现INotifyCompletion接口
            //此方法在任务完成时调用，这里就是在任务完成时调用OnCompleteAwaiter本地方法
            awaiter.OnCompleted(OnCompleteAwaiter);
            //本地函数
            void OnCompleteAwaiter()
            {
                Console.WriteLine(awaiter.GetResult());
                TraceThreadAndTask($"ended {nameof(CallerWithAwaiter)}");
            }
        }

        public static void CallerWithContinuationTask()
        {
            TraceThreadAndTask("started CallerWithContinuationTask");
            var t1 = GreetingAsync("Stephanie");
            //Task的ContinueWith方法定义了任务完成后调用的代码，ContinueWith方法的委托接受将已完成的任务作为参数传入
            //使用Result属性可以访问任务返回的结果
            t1.ContinueWith(t =>
            {
                string result = t.Result;
                Console.WriteLine(result);
                TraceThreadAndTask("ended CallerWithContinuationTask");
            });
        }

        private readonly static Dictionary<string, string> names = new Dictionary<string, string>();

        //ValueTask 是一个结构，比Task性能要好
        private static async ValueTask<string> GreetingValueTaskAsync(string name)
        {
            if(names.TryGetValue(name, out string result))
            {
                return result;  //返回ValueTask吗，直接返回结果即可，但是方法要加async
            }
            else
            {
                result = await GreetingAsync(name);
                names.Add(name, result);
                return result;
            }
        }

        public static async void UseValueTask()
        {
            string result = await GreetingValueTaskAsync("Katharina");
            Console.WriteLine(result);
            string result2 = await GreetingValueTaskAsync("Katharina");
            Console.WriteLine(result2);
        }

        //如果方法不适用async修饰符，而需要返回ValueTask
        //可以使用传递结果或者传递Task对象的构造函数创建ValueTask对象
         static  ValueTask<string> GreetingValueTask2Async(string name)
        {
            if (names.TryGetValue(name, out string result))
            {
                return new ValueTask<string>(result); //方法没Async，直接用构造函数转成ValueTask
            }
            else
            {
                Task<string> t1 = GreetingAsync(name);
                TaskAwaiter<string> awaiter = t1.GetAwaiter();
                awaiter.OnCompleted(OnCompletion); //Task执行的回调
                return new ValueTask<string>(t1); //用ValueTask构造函数转Task

                void OnCompletion()
                {
                    names.Add(name, awaiter.GetResult());
                }
            }
        }

        //将普通的异步模式转化为任务异步模式
        public static async void ConvertingAsyncPattern()
        {
            HttpWebRequest request = WebRequest.Create("http://www.baidu.com") as HttpWebRequest;

            using (WebResponse response =
                //Task.Factory.FromAsync<>()是一个泛型方法，提供一些重载版本，将异步模式转换为基于任务的异步模式
                //BeginXXX这样返回IAsyncResult信号的方法是第一个参数，
                //EndXXXX这样有一个IAsyncResult的参数的方法，是第二个参数
                await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse(null, null), request.EndGetResponse))
            {
                Stream stream = response.GetResponseStream();
                using (var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content.Substring(0, 100));
                }
            }
        }

        public async static void MultipleAsyncMethod()
        {
            Task<string> t1 = GreetingAsync("Tom");
            Task<string> t2 = GreetingAsync("Jack");
            //使用WhenAll并发两个Task，并且使用T[]获取返回值
            string[] result = await Task.WhenAll(t1, t2);
            //...
        }

    }
}
