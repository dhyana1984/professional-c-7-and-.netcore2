using System;
namespace ObjectAndClass
{
    public class ObjectAndClass
    {
        private string _name;
        private int _age;
        public ObjectAndClass(string name,int age)
        {
            _name = name;
            _age = age;
        }
        //:this(name, 20)是构造函数初始化器，在调用ObjectAndClass(string name)之前调用
        //如果是调用基类的构造函数作为构造函数初始化器，使用base(xx)       
        public ObjectAndClass(string name):this(name, 20)
        {
            _name = name;
        }

        private string _firstName;
        //定义属性使用表达式体，属性访问器只能一条语句
        //get和set中必须由一个具有属性本身的访问级别，否则会报错
        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }

        //自动实现只读属性
        public string Id { get; } = Guid.NewGuid().ToString();

        //表达式属性,只有get访问器才能用
        public string MyID => $"{FirstName} {Id}";

        public void CreateAnoymousObject()
        {
            //匿名对象，无法类型反射
            var anoymoutsObj1 = new
            {
                FistName = "AAA",
                MiddleName = "BBB",
                LastName = "CCC"
            };
        }

        //个数可变的参数，参数用params int[] data
        //如果是传递不同类型的参数，用params object[] data
        public void AnyNumberOfArguments(params int[] data)
        {
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }

        public void NullableType()
        {
            //可空类型赋值
            int? x1 = 1;
            int x2 = 2;
            //x2 = x1.HasValue ? x1.Value : -1;
            x2 = x1 ?? -1;

        }
    }
}
