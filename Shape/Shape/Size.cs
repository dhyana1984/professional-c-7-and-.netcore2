using System;

namespace MyShape
{
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public override string ToString() =>
            $"Width:{Width}, Height:{Height}";
    }
}