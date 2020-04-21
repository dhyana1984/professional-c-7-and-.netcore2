using System;

namespace OperatorAndCasting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //使用checked检查溢出
            byte b = 255;
            //使用checked后CLR会检查溢出
            checked //如果不要检查，使用unchecked
            {
                try
                {
                    //此时会抛出overflow的异常
                    b++;
                }
                catch (OverflowException e)
                {
                    Console.WriteLine("Error!" + e.Message);
                }
            }
            //Console.WriteLine(b);

            //Vector vector1, vector2, vector3;
            //vector1 = new Vector(1, 1.5, 2.0);
            //vector2 = new Vector(0.0, 0.0, -10.0);
            //vector3 = vector1 + vector2;
            //Console.WriteLine($"vector1 = {vector1}");
            //Console.WriteLine($"vector2 = {vector2}");
            //Console.WriteLine($"vector3 = {vector3}");
            //Console.WriteLine($"2 * vector3 = { 2 * vector3}");
            //Console.WriteLine($"vector2 * 3= {vector2 * 3}");
            //Console.WriteLine($"vector3 += vector2 gives {vector3 += vector2}");
            //Console.WriteLine($"vector3 *= vector1 * 2 gives {vector3 = vector1 * 2}");
            //Console.WriteLine($"vector1 * vector3 = {vector1 * vector3}");
            //Console.WriteLine($"vector1 == vector2 returns {vector1 == vector2}");
            //Console.WriteLine($"vector2 != vectror1 returns {vector2 != vector1}");
            var p1 = new Person("Tom", "Jason", new DateTime(1992, 2, 3));
            var p2 = new Person("Jim", "Lee", new DateTime(1993, 5, 6));
            var p3 = new Person("Jack", "Wong", new DateTime(1997, 8, 9));
            var coll = new PersonCollection(p1, p2, p3);
            Console.WriteLine(coll[2]);//Jack Wong
            foreach (var item in coll[new DateTime(1993,5,6)])
            {
                Console.WriteLine(item); // Jim Lee
            }

        }
    }
}
