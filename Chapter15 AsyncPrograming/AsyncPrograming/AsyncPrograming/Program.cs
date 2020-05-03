using System;
using System.Threading.Tasks;

namespace AsyncPrograming
{
    class Program
    {
        static void Main(string[] args)
        {
            //AsyncMode.AsynchronousPattern();
            //AsyncMode.EventBasedAsyncPattern();
            //await AsyncMode.TaskBasedAsynchronousPatternAsync();
            //AsyncPrograming.CallerWithAsync();
            //AsyncPrograming.CallerWithAwaiter();
            //AsyncPrograming.CallerWithContinuationTask();
            //AsyncPrograming.UseValueTask();
            //AsyncPrograming.ConvertingAsyncPattern();
            TaskException.StartTwoTasks();
            Console.ReadLine();
        }
    }
}
