using System;
using System.Collections.Generic;

namespace ArraySample
{
    public class MusicTitles
    {
        string[] names = {"嚣张", "年少有为", "山楂树之恋", "突然好想你", "疑心病"};

        //GetEnumerator方法是类的默认迭代方式，不用调用GetEnumerator方法即可直接获取迭代器
        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < names.Length; i++)
            {
                yield return names[i];
            }
        }

        public IEnumerable<string> Reverse()
        {
            for (int i = names.Length-1; i >=0; i--)
            {
                yield return names[i];
            }
        }

        public IEnumerable<string> Subset(int index, int length)
        {
            for (int i = index; i <index + length; i++)
            {
                yield return names[i];
            }
        }

        
    }
}
