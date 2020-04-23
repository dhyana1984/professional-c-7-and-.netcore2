using System;
namespace DelegateLambdaEvent
{
    public class MathOperations
    {
        public static double MultiplayByTwo(double value) => value * 2;
        public static double Square(double value) => value * value;

        public static void ActionMultiplayByTwo(double value)
        {
            double result = value * 2;
            Console.WriteLine($"ActionMultiplayByTwo by 2: {value} gives {result}");
        }

        public static void ActionSquare(double value)
        {
            double result = value * value;
            Console.WriteLine($"Squaring: {value} gives {result}");
        }
    }
}
