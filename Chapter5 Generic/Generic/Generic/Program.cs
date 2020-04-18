using System;
using System.Collections.Generic;
using MyShape;

namespace Generic
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dm = new DocumentManager<Document>();
            //dm.AddDocument(new Document("TitleA", "ContentA"));
            //dm.AddDocument(new Document("TitleB", "ContentB"));
            //dm.DisplayAllDocuments();
            //if (dm.IsDocumentAvaliable)
            //{
            //    Document d = dm.GetDocument();
            //    Console.WriteLine(d.Content);
            //}

            var accounts = new List<Account>
            {
                new Account("Tom",1500),
                new Account("Bob",2200),
                new Account("John",1800),
                new Account("Marry",2400),
                new Account("Lily",3800),
            };
            //不用Algirithms.Accumulate<Account>，编译器会自动推断泛型的类型是Account
            //decimal amount = Algirithms.Accumulate(accounts);
            //Console.WriteLine(amount);

            //decimal amount = Algirithms.AccumulateDelegate<Account, decimal>(
            //    accounts, (item, sum) => sum += item.Balance); // (item, sum) => sum += item.Balance就是传入Func<T1, T2, T2> action的参数
            //Console.WriteLine(amount);


            IIndex<Rectangle> rectangles = RectangleCollection.GetRectangles();
            //因为接口IIndex是协变的，所以可以返回值赋予IIndex<Shape>类型的变量
            IIndex<Shape> shapes = rectangles;
            for (int i = 0; i < shapes.Count; i++)
            {
                Console.WriteLine($"Shape {i},Width: {shapes[i].Width}, Height: {shapes[i].Height}");
            }
            IDisplay<Shape> shapeDisplay = new ShapeDisplay();
            //因为IDisplay接口是抗变的，所以可以把结果赋予IDisplay<Rectangle>
            IDisplay<Rectangle> rectangleDisplay = shapeDisplay;
            rectangleDisplay.Show(rectangles[0]);
        }
    }
}
