using System;
namespace FunctionalPrograming
{
    public static class TupleSample
    {
        public static void IntroTuples()
        {
            //创建tuple
            (string s, int i, PersonSample p) t = ("magic", 42, new PersonSample("Jim Ben"));
            //通过元组的成员名读取tuple的属性
            Console.WriteLine($"s: {t.s}, i: {t.i}, p: {t.p}");

            //直接创建元组
            var t2 = ("magic", 42, new PersonSample("James Bond"));
            //通过Item读取tuple的属性
            Console.WriteLine($"s: {t2.Item1}, i: {t2.Item2}, p: {t2.Item3}");

            //通过为元组中的字面量指定名称来声明
            var t3 = (s:"magic", i:42, p: new PersonSample("Karl Tom"));
            //通过字面量的名称读取tuple的属性
            Console.WriteLine($"s: {t3.s}, i: {t3.i}, p: {t3.p}");
        }

        public static void TupleDecunstruction()
        {
            //解构tuple并赋值到变量
            (var s1, var i1, var p1) = ("magic", 42, new PersonSample("Stephanie Nagel"));
            Console.WriteLine($"s: {s1}, i: {i1}, p: {p1}");

            //先定义变量，再解构赋值
            string s2;
            int i2;
            PersonSample p2;
            (s2, i2,p2) = ("magic", 42, new PersonSample("Stephanie Nagel"));
            Console.WriteLine($"s: {s2}, i: {i2}, p: {p2}");


            //如果不需要元组所有部分，可以使用_忽略该部分
            (_, _, var p3) = ("magic", 42, new PersonSample("Stephanie Nagel"));
            Console.WriteLine($"p: {p3}");
        }

        public static void TupleBackend()
        {
            //(var s, var i) = ("magic", 42);//元组字面量

            //元组实现。利用ValueTuple.Create
            ValueTuple<string, int> t2 = ValueTuple.Create("magic", 42);
        }
    }
}
