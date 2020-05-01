using System;

namespace FunctionalPrograming
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonSample person = new PersonSample("Chris Xiong");
            Console.WriteLine(person.ToString());

            //using (var r = new Resource())
            //{
            //    r.Foo();
            //}
            //使用Use扩展方法实现using
            new Resource().Use(r => r.Foo());
            TupleSample.IntroTuples();
            TupleSample.TupleDecunstruction();

            var p1 = new PersonSample("Chris Xiong");
            (var firsName, var lastName) = p1;
            Console.WriteLine($"PersonSample class decunstruct as: FirstName: {firsName} LastName: {lastName}");
            ModeMatchSample.ModeMatchSampleDisplay();

        }
    }
}
