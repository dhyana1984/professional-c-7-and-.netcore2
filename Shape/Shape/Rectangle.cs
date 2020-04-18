using System;

namespace MyShape
{
    public class Rectangle : Shape
    {

        public override void Draw()
        {
            base.Draw();
            Console.WriteLine($"Rectangle with {Position} and {Size}");
        }

        new public void MyMethod()
        {
            Console.WriteLine("New MyMethod");
        }
    }
}