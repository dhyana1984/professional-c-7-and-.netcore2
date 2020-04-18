using System;

namespace MyShape
{
    public class Rectangle : Shape
    {
        public Rectangle()
        {

        }

        public override void Draw()
        {
            base.Draw();
            Console.WriteLine($"Rectangle with {Position} and {Size}");
        }

    }
}