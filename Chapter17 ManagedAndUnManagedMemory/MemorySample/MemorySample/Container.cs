using System;
using System.Linq;

namespace MemorySample
{
    public class Container
    {
        public Container(int[] data) => _data = data;
        private int[] _data;

        //返回index项的引用
        public ref int GetItem(int index) => ref _data[index];

        //返回完整数组引用
        public ref int[] GetData() => ref _data;

        public void ShowAll()
        {
            Console.WriteLine(string.Join(", ",_data));
        }
     
    }

    public static class ContainerSample
    {
        public static void UseItemOfContainer()
        {
            Console.WriteLine(nameof(UseItemOfContainer));
            var c = new Container(Enumerable.Range(0, 10).Select(x => x).ToArray());
            //item就是第4项的引用
            ref int item = ref c.GetItem(3);
            //改变后就直接修改了Container中的_data数组
            item = 33;
            c.ShowAll();
        }

        public static void UseArrayOfContainer()
        {
            Console.WriteLine(nameof(UseArrayOfContainer));
            var c = new Container(Enumerable.Range(0, 10).Select(x => x).ToArray());
            //获得Constainer中_data数组的引用
            ref int[] d1 = ref c.GetData();
            //修改数组的值
            d1 = new int[] { 4, 5, 6 };
            c.ShowAll();
        }
    }
}
