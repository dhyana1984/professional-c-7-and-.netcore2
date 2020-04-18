using System;
using MyShape;

namespace Generic
{

    public class RectangleCollection : IIndex<Rectangle>
    {
        private Rectangle[] data = new Rectangle[3]
        {
            new Rectangle{Height=2, Width=5},
            new Rectangle{Height=3, Width=7},
            new Rectangle{Height=4, Width=9},
        };

        private static RectangleCollection _coll;
        public static RectangleCollection GetRectangles() =>
            _coll ?? (_coll = new RectangleCollection());

        public Rectangle this[int index]
        {
            get
            {
                if(index<0 || index > data.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return data[index];
            }
        }

        public int Count => data.Length;
    }

    public class ShapeDisplay : IDisplay<Shape>
    {
        public void Show(Shape s) =>
            Console.WriteLine($"{s.GetType().Name} Width: {s.Width}, Height: {s.Height}");
    }
}
