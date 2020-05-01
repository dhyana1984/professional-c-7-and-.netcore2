using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalPrograming
{
    public static class ModeMatchSample
    {

        public static void ModeMatchSampleDisplay()
        {
            var p1 = new PersonSample("Tom Jason");
            var p2 = new PersonSample("Jack Kola");
            object[] data = { null, 42, "abstract", p1, new PersonSample[] { p1, p2 } };
            foreach (var item in data)
            {
                IsOperator(item);
            }

            foreach (var item in data)
            {
                SwitchStatement(item);
            }

        }

        private static void IsOperator(object item)
        {
            //is运算符匹配常量
            if(item is null)
            {
                Console.WriteLine("item is null");
            }
            if(item is 42)
            {
                Console.WriteLine("item is 42");
            }

            //is运算符匹配type
            if(item is int)
            {
                Console.WriteLine($"item is of type int");
            }
            if(item is int i) //如果匹配上，则把item赋给i以便在代码块内使用
            {
                Console.WriteLine($"item is of type int with the value {i}");
            }
            if (item is string s)
            {
                Console.WriteLine($"Item is a string: {s}");
            }
            if(item is PersonSample p && p.FirstName.StartsWith("To"))//匹配成功的话，可以通过赋值的p做一些附加条件
            {
                Console.WriteLine($"Item is a person:{p.FirstName} {p.LastName}");
            }
            if (item is IEnumerable<PersonSample> people)
            {
                string names = string.Join(", ", people.Select(p1 => p1.FirstName));
                Console.WriteLine($"it's a Person collection containing {people}");
            }

            //匹配var
            if(item is var every)
            {
                Console.WriteLine($"it's var of type {every?.GetType().Name ?? "null"} with the value {every?? "nothing"}");
            }

        }

        private static void SwitchStatement(Object item)
        {
            //switch匹配模式
            switch (item)
            {
                case null:
                case 42:
                    Console.WriteLine("it's a const pattern");
                    break;
                case int i:
                    Console.WriteLine($"it's a type pattern with int: {i}");
                    break;
                case string s:
                    Console.WriteLine($"it's a type pattern with string: {s}");
                    break;
                    //使用when增加筛选条件
                case PersonSample p when p.FirstName == "Tom":
                    Console.WriteLine($"type pattern match with Person and when clause: {p}");
                    break;
                case PersonSample p:
                    Console.WriteLine($"type pattern match with Person: {p}");
                    break;
                case var every:
                    Console.WriteLine($"var pattern match: {every?.GetType().Name}");
                    break;
                default:
            }    
        }
    }
}
