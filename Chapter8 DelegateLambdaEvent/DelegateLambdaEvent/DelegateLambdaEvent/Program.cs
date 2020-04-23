using System;
using Lib.Employee;
using OperatorAndCasting;

namespace DelegateLambdaEvent
{
    class Program
    {
        private delegate string GetAString();
        private delegate double DoubleOp(double x);
        static void Main(string[] args)
        {
            int x = 40;
            GetAString firstStringMethod = x.ToString;
            Console.WriteLine($"String is {firstStringMethod()}");
            var balance = new Currency(34, 50);

            firstStringMethod = balance.ToString;
            Console.WriteLine($"String is {firstStringMethod()}");

            firstStringMethod = new GetAString(Currency.GetCurrencyUnit);
            Console.WriteLine($"String is {firstStringMethod()}");
            Console.WriteLine($"------------------");
            DoubleOp[] operations =
            {
                MathOperations.MultiplayByTwo,
                MathOperations.Square
            };
            //声明参数类型是double,返回类型是double的Func委托，
            //对于Func<a,b>, b类型是返回类型，a类型是输入参数类型，输入参数类型可以有多个
            //例如Func<a,a,c,b>，表示3个参数，分别是a类型，a类型，c类型,返回值是b类型
            Func<double, double>[] funcOperations =
            {
                MathOperations.MultiplayByTwo,
                MathOperations.Square
            };
            for (int i = 0; i < operations.Length; i++)
            {

                Console.WriteLine($"Using operation[{i}]");
                ProcessAndDisplayNumber(operations[i], 2.0);
                ProcessAndDisplayNumber(operations[i], 7.94);
                ProcessAndDisplayNumber(operations[i], 1.414);
            }

            for (int i = 0; i < funcOperations.Length; i++)
            {

                Console.WriteLine($"Using operation[{i}]");
                ProcessFuncAndDisplayNumber(funcOperations[i], 2.0);
                ProcessFuncAndDisplayNumber(funcOperations[i], 7.94);
                ProcessFuncAndDisplayNumber(funcOperations[i], 1.414);
            }

            Console.WriteLine($"------------------");

            Employee[] employees =
            {
                new Employee("Tom",20000),
                new Employee("Jack",10000),
                new Employee("Bob",15000),
                new Employee("Roy",100000.38m),
                new Employee("Lily",23000),
                new Employee("Jean",50000),
            };
            //将Employee.CompareSalary方法作为委托参数传入BubbleSorter.Sort方法
            BubbleSorter.Sort(employees, Employee.CompareSalary);
            foreach (var item in employees)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"------------------");

            //多播委托，一个委托可以包含多个方法，但是委托签名必须返回void，否则只能得到委托调用最后一个方法的结果
            Action<double> actionOperations = MathOperations.ActionMultiplayByTwo;
            actionOperations += MathOperations.ActionSquare;
            ProcesActionAndDisplayNumber(actionOperations, 2.0);
            ProcesActionAndDisplayNumber(actionOperations, 7.94);
            ProcesActionAndDisplayNumber(actionOperations, 1.414);
            Console.WriteLine($"------------------");

            string mid = ", middle part,";
            //匿名方法
            //   Func<string, string> anonDel = delegate(string param)
            //   {
            //       param += mid;
            //       param += " and this was added to the string";
            //       return param;
            //};
            //使用lambda表达式的匿名方法，参数的类型和委托定义的类型对照
            Func<string, string> anonDel = param =>
            {
                param += mid;
                param += " and this was added to the string";
                return param;
            };

            Console.WriteLine(anonDel("Start of string"));

            Console.WriteLine($"------------------");

            var dealer = new CarDealer();
            var valtteri = new Consumer("Valtteri");
            dealer.NewCarInfo += valtteri.NewCarIsHere;
            dealer.NewCar("Williams");

            var max = new Consumer("Max");
            dealer.NewCarInfo += max.NewCarIsHere;
            dealer.NewCar("Mercedes");
            dealer.NewCarInfo -= valtteri.NewCarIsHere;
            dealer.NewCar("Ferrari");
        }

        //执行普通委托
        static void ProcessAndDisplayNumber(DoubleOp action, double value)
        {
            //调用委托
            double result = action(value);
            Console.WriteLine($"Value is {value}, result of operation is {result}");
        }
        //执行Func<>委托
        static void ProcessFuncAndDisplayNumber(Func<double, double> action, double value)
        {
            //调用委托
            double result = action(value);
            Console.WriteLine($"Value is {value}, result of operation is {result}");
        }
        //执行Action<>多播委托
        static void ProcesActionAndDisplayNumber(Action<double> action, double value)
        {
            //调用委托
            Console.WriteLine();
            Console.WriteLine($"ProcesActionAndDisplayNumber called with value = {value}");
            action(value);
        }
    }
}
