using System;
namespace Inherit
{
    public class VirtualMethod
    {
        public VirtualMethod()
        {
        }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        //重写基类Object中的ToString方法，Object的ToString方法默认是virtual方法
        public override string ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public override string ToString() =>
            $"Width:{Width}, Height:{Height}";
    }

    public class Shape
    {
        public Position Position { get; } = new Position();
        public Size Size { get; } = new Size();

        //虚方法可以在子类利用通过override关键字重写
        public virtual void Draw() => Console.WriteLine($"Shape with {Position} and {Size}");

        public void MyMethod()
        {

        }
    }

    public class Rectangle : Shape
    {
        //使用override重写父类定义的virtual方法
        public override void Draw()
        {
            //使用base.<MethodName>调用基类的方法
            base.Draw();
            Console.WriteLine($"Rectangle with {Position} and {Size}");
        }

        //子类中使用new关键字隐藏父类相同签名的方法
        new public void MyMethod()
        {
            Console.WriteLine("New MyMethod");
        }
        
   
    }
}
