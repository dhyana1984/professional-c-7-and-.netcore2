using System;
using Lib.Person;

namespace StringSample
{
    //IFormattable接口实现自定义格式化字符串
    public class FormatPerson : Person, IFormattable
    {
        public FormatPerson(string firstName, string lastName, DateTime birthDay)
            :base(firstName, lastName, birthDay)
        {

        }

        public override string ToString() => FirstName + " " + LastName;
        public virtual string ToString(string format) => ToString(format, null);

        //此方法是实现IFormattable接口的方法，第一个参数就是格式的字符串参数
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case null:
                case "A":
                    return ToString();
                case "F":
                    return FirstName;
                case "L":
                    return LastName;
                default:
                    throw new FormatException($"invalid format string {format}");
            }
        }
    }
}
