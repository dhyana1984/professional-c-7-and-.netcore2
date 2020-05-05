using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace ClientApp
{
    
    class Program
    {
        private const string CalculatorTypeName = "CaculatorLib.Calculator";
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowUsage();
                return;
            }
            UsingReflection(args[0]);
            UsingReflectionWithDynamic(args[0]);
        }

        private static void ShowUsage()
        {
            Console.WriteLine($"Usage: {nameof(ClientApp)} path");
            Console.WriteLine();
            Console.WriteLine("Copy CalculatorLib.dll to an addin directory");
            Console.WriteLine("and pass the absolute path of this directory when starting the application to load the library");
        }

        private static object GetCalculator(string addinPath)
        {
            Assembly assembly = Assembly.LoadFile(addinPath);
            //创建实例
            var res =  assembly.CreateInstance(CalculatorTypeName);
            return res;
        }

        private static void UsingReflection(string addinPath)
        {
            double x = 3;
            double y = 4;
            object calc = GetCalculator(addinPath);
            //dynamic calc = GetCalculator(addinPath);

            //Invoke类似javascrupt的apply
            object result = calc.GetType().GetMethod("Add").Invoke(calc, new object[] { x, y });
            Console.WriteLine($"the result of {x} and {y} is {result}");
        }

        private static void UsingReflectionWithDynamic(string addinPath)
        {
            double x = 3;
            double y = 4;
            dynamic calc = GetCalculator(addinPath);
            //使用dynamic类型定义 assembly.CreateInstance("Calculator")的返回值，可以直接调用Add()方法
            double result = calc.Add(x, y);
            Console.WriteLine($"the result of {x} and {y} is {result}");


            try
            {
                result = calc.Multiply(x, y);
            }
            catch (RuntimeBinderException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
