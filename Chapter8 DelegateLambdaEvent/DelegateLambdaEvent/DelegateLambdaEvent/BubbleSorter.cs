using System;
using System.Collections;
using System.Collections.Generic;

namespace DelegateLambdaEvent
{
    public class BubbleSorter
    {
        // Func<T, T, bool> comparison是传进来的排序方法的委托，传入两个T类型参数，返回bool类型结果
        //对应Employee中的 public static bool CompareSalary(Employee e1, Employee e2) 排序方法
        static public void Sort<T>(IList<T> sortArray, Func<T, T, bool> comparison)
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for (int i = 0; i < sortArray.Count - 1; i++)
                {
                    if (comparison(sortArray[i + 1], sortArray[i]))
                    {
                        T temp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
    }
}
