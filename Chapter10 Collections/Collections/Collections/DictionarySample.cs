using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Employee;

namespace Collections
{
    public class DictionarySample
    {

        Dictionary<int, string> _dict = new Dictionary<int, string>()
        {
            //这是C#提供的初始化Dictionary的语法
            [3] = "three",
            [7] = "seven"
        };

        public void DisplaySample()
        {
            
            var idJimmie = new EmployeeId("C48");
            var jimmie = new DictEmployee(idJimmie, "Jimmie Johnson", 150926.00m);

            var idJoey = new EmployeeId("F22");
            var joey = new DictEmployee(idJoey, "Joey Logano", 45125.00m);

            var idKyle = new EmployeeId("T18");
            var kyle = new DictEmployee(idKyle, "Kyle Bush", 78728.00m);

            var idCarl = new EmployeeId("T19");
            var carl = new DictEmployee(idCarl, "Carl Edwards", 80473.00m);

            var idMatt = new EmployeeId("T20");
            var matt = new DictEmployee(idMatt, "Matt Kenseth", 113970.00m);

            var employees = new Dictionary<EmployeeId, Employee>(31)
            {
                //以EmployeeId的实例为键，值是DictEmployee类型
                [idJimmie] = jimmie,
                [idJoey] = joey,
                [idKyle] = kyle,
                [idCarl] = carl,
                [idMatt] = matt
            };

            foreach (var employee in employees.Values)
            {
                Console.WriteLine(employee);
            }
        }

        public class EmployeeIdException : Exception
        {
            public EmployeeIdException(string message) : base(message) { }
        }

        public class DictEmployee : Employee
        {
            private readonly EmployeeId _id ;
            private string _name;
            private decimal _salary; 
            public DictEmployee(EmployeeId id, string name, decimal salary): base(name, salary)
            {
                _id = id;
                _name = name;
                _salary = salary;
            }

            public override string ToString() =>
                //:C不能有空格
                $"{_id.ToString()}: {_name,-20} {_salary:C}";
        }

        public class EmployeeId : IEquatable<EmployeeId>
        {

            private readonly char _prefix;
            private readonly int _number;

            public EmployeeId(string id)
            {
                if (id == null) throw new ArgumentNullException(nameof(id));
                _prefix = (id.ToUpper())[0];
                int numLength = id.Length - 1;
                try
                {
                    _number = int.Parse(id.Substring(1, numLength > 6 ? 6 : numLength));
                }
                catch (FormatException)
                {
                    throw new EmployeeIdException("Invalid Employeeid format");
                }
            }

            public override string ToString() => _prefix.ToString() + $"{_number,6:000000}";

            //实现IEquatable<EmployeeId>接口的Equals方法，定义GetHashCode的逻辑
            //(_number ^ _number << 16) * 0x15051505 算法是为了让散列码在整数取值区域上分布均匀
            public override int GetHashCode() => (_number ^ _number << 16) * 0x15051505;

           //真正的键值的比较方法而不是通过引用地址比较
            public bool Equals(EmployeeId other) =>
                (_prefix == other?._prefix && _number == other?._number);

            //实现IEquatable<EmployeeId>接口的Equals方法，作为键查找散列值得比较逻辑，而不是通过引用比较
            public override bool Equals(object other) =>
                Equals((EmployeeId)other);

            public static bool operator ==(EmployeeId left, EmployeeId right) =>
                left.Equals(right);

            public static bool operator !=(EmployeeId left, EmployeeId right) =>
               !(left == right);

        }

        
    }
}
