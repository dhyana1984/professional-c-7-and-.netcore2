using System;

namespace Inherit
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Rectangle();
            r.Position.X = 1;
            r.Position.Y = 2;
            r.Size.Width = 3;
            r.Size.Height = 4;
            r.Draw();
        }
    }
}
