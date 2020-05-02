using System;
using System.Threading.Tasks;

namespace AsyncPrograming
{
    public static class TaskException
    {
        static async Task ThrowAfter(int ms, string message)
        {
            await Task.Delay(ms);
            throw new Exception(message);
        }

       public static async void HandleOneError()
        {
            try
            {
               //在try里面用await让方法释放线程，捕获Task的异常
               await ThrowAfter(200, "first");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async void StartTwoTasks()
        {
            Task taskResult = null; 
            try
            {
                Task t1 = ThrowAfter(2000, "first");
                Task t2 = ThrowAfter(2000, "second");

               
                //多个Task的话，如果没有依赖关系，使用WhenAll来运行
                //在try外部定义一个Task对象来接受WhenAll的返回值便于捕获
                await ( taskResult =  Task.WhenAll(t1, t2));
            }
            catch(Exception ex)
            {
                Console.WriteLine("handled " + ex.Message);
                //遍历 taskResult.Exception.InnerExceptions以获得所有出错的Task
                //现在这个Exception是AggregateException
                foreach (var item in taskResult.Exception.InnerExceptions)
                {
                    Console.WriteLine($"inner exception {item.Message}");
                }
            }
        }
    }
}
