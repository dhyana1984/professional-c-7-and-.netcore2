using System;
namespace Inherit
{
    public class AbstractClassAndMethod
    {
        public AbstractClassAndMethod()
        {
        }
    }

    //包含抽象方法的抽象类
    public abstract class AbStractShape
    {
        //抽象方法不能被实现
        public abstract void Resize(int width, int height);
        public Position Position { get; } = new Position();
        public Size Size { get; } = new Size();
    }

    public class Ellipse : AbStractShape
    {
        //使用override实现抽象方法
        public override void Resize(int width, int height)
        {
            Size.Width = width;
            Size.Height = height;
        }
    }
}
