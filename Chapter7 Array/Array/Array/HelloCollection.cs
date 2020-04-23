using System;
using System.Collections;
using System.Collections.Generic;

namespace ArraySample
{
    public class HelloCollection
    {
        //GetEnumerator用IEnumerable定义，但是并不需要一定在类中实现IEnumerable接口
        //只要实现了返回IEnumerator接口的GetEnumerator方法，就可以被foreach遍历
        public IEnumerator<string> GetEnumerator()
        {
            //包含yield的代码块叫做迭代块，迭代块必须声明为返回IEnumerator或IEnumerable接口
            //可以包含多条yield return或者yield break，但是    不能包含return语句
            yield return "Hello";
            yield return "World";
        }

        public void HelloWorld()
        {
            var helloCollection = new HelloCollection();
            foreach (var item in helloCollection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
