using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataLib;

namespace LINQSample
{
    public static class ExpressionSample
    {

        //IEnumerable<T>的Where扩展方法的实现
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        //IQueryable<T>的Where扩展方法实现
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {

            if (source == null)
            {
                throw new ArgumentException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentException("predicate");
            }
            return source.Provider.CreateQuery<TSource>(Expression.Call(
                    null,
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }),
                    new Expression[] { source.Expression, Expression.Quote(predicate) }
                ));
        }


        public static void DisplaySample()
        {
            Expression<Func<Racer, bool>> expression = r => r.Country == "Brazil" && r.Wins > 6;
            DisplayTree(0, "Lambda", expression);
        }

        static void DisplayTree(int indent, string message, Expression expression)
        {
            string output = $"{string.Empty.PadLeft(indent, '>')} {message} ! NodeType: {expression.NodeType}; Expr: {expression}";
            indent++;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    Console.WriteLine(output);
                    LambdaExpression lambdaExpression = (LambdaExpression)expression;
                    foreach (var parameter in lambdaExpression.Parameters)
                    {
                        DisplayTree(indent, "Parameter", parameter);
                    }
                    DisplayTree(indent, "Body", lambdaExpression.Body);
                    break;
                case ExpressionType.Constant:
                    ConstantExpression constantExpression = (ConstantExpression)expression;
                    Console.WriteLine($"{output} Const Value: {constantExpression.Value}");
                    break;
                case ExpressionType.Parameter:
                    ParameterExpression parameterExpression = (ParameterExpression)expression;
                    Console.WriteLine($"{output} Param Type: {parameterExpression.Type.Name}");
                    break;
                case ExpressionType.Equal:
                case ExpressionType.AndAlso:
                case ExpressionType.GreaterThan:
                    BinaryExpression binaryExpression = (BinaryExpression)expression;
                    if(binaryExpression.Method != null)
                    {
                        Console.WriteLine($"{output} Method: {binaryExpression.Method.Name}");
                    }
                    else
                    {
                        Console.WriteLine(output);
                    }
                    DisplayTree(indent, "Left", binaryExpression.Left);
                    DisplayTree(indent, "Right", binaryExpression.Right);
                    break;
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)expression;
                    Console.WriteLine($"{output} Member Name: {memberExpression.Member.Name}, Type: {memberExpression.Expression}");
                    DisplayTree(indent, "Member Expr", memberExpression.Expression);
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine($"{expression.NodeType} {expression.Type.Name}");
                    break;
            }

        }
    }
}
