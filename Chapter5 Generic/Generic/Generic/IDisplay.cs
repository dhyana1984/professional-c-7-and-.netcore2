using System;
namespace Generic
{
    //接口泛型使用关键字in，泛型接口就是抗变的
    public interface IDisplay<in T>
    {
        void Show(T item);
    }
}
