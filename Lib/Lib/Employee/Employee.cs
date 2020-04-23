using System;
namespace Lib.Employee
{
    public class Employee
    {
        public Employee(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }

        public string Name { get; }
        public decimal Salary { get; }

        public override string ToString() =>
            $"{Name}, {Salary:C}";
        //为了匹配BubbleSorter类中static public void Sort<T>(IList<T> sortArray, Func<T, T, bool> comparison)方法的comparison参数
        //此方法作为委托传入该方法
        public static bool CompareSalary(Employee e1, Employee e2) =>
            e1.Salary < e2.Salary;
    }
}
