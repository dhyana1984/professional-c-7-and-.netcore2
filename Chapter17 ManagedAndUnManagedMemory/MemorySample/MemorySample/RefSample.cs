using System;
namespace MemorySample
{
    public class RefSample
    {
        public RefSample()
        {
        }
    }

    public class Data
    {
        private int _anumber;
        public Data(int anumber) => _anumber = anumber;

        //作为引用返回
        public ref int GetNumber() => ref _anumber;

        public ref readonly int GetReadonlyNumber() => ref _anumber;

        public void Show() => Console.WriteLine($"Data: {_anumber}");


    }
}
