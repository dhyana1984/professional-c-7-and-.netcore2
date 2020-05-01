using System;
namespace FunctionalPrograming
{
    public static class LocalFunctionSample
    {

        public static void IntroWithLambdaExpression()
        {
            //使用委托定义
            Func<int, int, int> addByFunc = (x, y) =>
              {
                  return x + y;
              };

            //使用本地函数
            int addByLocalFunction(int x, int y)
            {
                return x + y;
            }

            //使用lambda表达式
            int addByLambda(int x, int y) => x + y;


            int resByFunc = addByFunc(1, 1);
            int resByLocalFunction = addByLocalFunction(2, 2);
            int resByLambda = addByLambda(3, 3);
            Console.WriteLine("resByFunc: "+ resByFunc);
            Console.WriteLine("resByLocalFunction: " + resByLocalFunction);
            Console.WriteLine("resByLambda: " + resByLambda);
        }
    }
}