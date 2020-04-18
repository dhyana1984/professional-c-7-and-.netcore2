﻿using System;

namespace MyShape
{
    public class Shape
    {
        public Position Position { get; } = new Position();
        public Size Size { get; } = new Size();
        public virtual void Draw() => Console.WriteLine($"Shape with {Position} and {Size}");

        public void MyMethod()
        {

        }
    }
}
