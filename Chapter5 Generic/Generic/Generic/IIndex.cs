using System;
namespace Generic
{
    //泛型类型用out关键字，泛型接口是协变的，返回类型只能是T
    public interface IIndex <out T>
    {
        T this [int index] { get; } //索引器
        int Count { get; }
    }
}
