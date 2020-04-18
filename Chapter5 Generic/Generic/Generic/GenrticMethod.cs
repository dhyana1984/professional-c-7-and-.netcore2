using System;
using System.Collections.Generic;

namespace Generic
{

    public class Account : IAccount
    {
        public string Name { get; }
        public decimal Balance { get; }

        public Account(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }
    }

    public interface IAccount
    {
        public string Name { get; }
        public decimal Balance { get; }
    }

    public static class Algirithms
    {
        public static decimal Accumulate<TAccount>(IEnumerable<TAccount> source) where TAccount : IAccount
        {
            decimal sum = 0;
            foreach (var item in source)
            {
                sum += item.Balance;
            }
            return sum;
        }

        //AccumulateDelegate<T1,T2>T1是传入的类型，T2是返回的类型
        //Func<T1, T2, T2>是泛型委托 Func<T1, T2, TResult>, TResult和T2是同样的类型
        //AccumulateDelegate方法传入两个参数，第一个是T1的集合类型，第二个是一个委托
        public static T2 AccumulateDelegate<T1,T2>(IEnumerable<T1> source, Func<T1, T2, T2> action)
        {
            T2 sum = default(T2);
            foreach (T1 item in source)
            {
                sum = action(item, sum);
            }
            return sum;
        }
    }
}
