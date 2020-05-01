using System;
namespace FunctionalPrograming
{
    public static class Extensions
    {
        //扩展string[]的ToStrings()方法，带出第二个参数和第三个参数
        public static void ToStrings(this string[] values, out string value1, out string value2)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length != 2) throw new IndexOutOfRangeException("only arrays with 2 values allowed");

            value1 = values[0];
            value2 = values[1];
        }

        public static void Use<T>(this T item, Action<T> action) where T: IDisposable
        {
            //扩展对一个类的using的方法，使用Use方法代替
            using (item)
            {
                action(item);
            }
        }
    }
}
