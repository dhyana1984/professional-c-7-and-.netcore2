using System;
using System.Collections.Generic;

namespace MemorySample
{
    class Program
    {
        class Item
        {
            public Item(int x)
            {
                Value = x;
            }
            public int Value
            {
                set;get;
            }
        }
        static void Main(string[] args)
        {
            //PointerPlayGround.DisplaySample();
            UserMember();
            UseMax();
            ContainerSample.UseItemOfContainer();
            ContainerSample.UseArrayOfContainer();

            var t1 = new Item(1);
            var t2 = new Item(2);
            var t3 = new Item(3);

            var t = t1;
            var list = new List<Item>();
            list.Add(t);
            t = t2;
            list.Add(t);
            t = t3;
            list.Add(t);
            list.ForEach(t => Console.WriteLine(t.Value));
        }

        static void UserMember()
        {
            Console.WriteLine(nameof(UserMember));
            var d = new Data(11);
            //没有用ref声明，不能改变原来的值
            //int n = d.GetNumber();
            //n = 42;
            //d.Show(); //d = 11

            //通过引用返回值类型，并且改变之后可以改变原来的值
            ref int n = ref d.GetNumber();
            n = 42;
            d.Show(); // d = 42

            ref readonly int n2 = ref d.GetReadonlyNumber();
            //n2 = 42; n2 is readonly
        }

        static void UseMax()
        {
            Console.WriteLine(nameof(UseMax));
            int x = 4, y = 5;
            //调用的时候也要加ref
            ref int z = ref Max(ref x, ref y);
            Console.WriteLine($"{z} is the max of {x} and {y}");
            z = x + y;
            //因为此时Max里面返回的是ref y，所以z就是y的引用
            //当z改变后，y也会有相应变化
            Console.WriteLine($"y after changing z: {y}");
        }

        //传递ref int并且返回ref int
        //如果不需要赋值x和y，将它们传递给方法Max，则可以快速返回，如果经常要调用此方法，这将非常有用
        //返回引用时很快的，因为幕后只使用指针，但是这也意味着可以更改引用指向原始项
        static ref int Max(ref int x, ref int y)
        {
            if (x > y) return ref x;
            else return ref y;
        }


           

    }
}
