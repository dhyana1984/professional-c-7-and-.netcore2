using System;

using Lib.Person;

namespace ArraySample
{
    //通过实现实IComparable<ComparablePerson>来现实ComparablePerson数组排序
    public class ComparablePerson : Person, IComparable<Person>
    {
        public ComparablePerson(string firstName, string lastName, DateTime birthday) : base(firstName, lastName, birthday)
        {

        }

        public int CompareTo(Person other)
        {
            if (other == null) return 1;
            //如果相等，返回0
            //如果该实例应排在参数对象前面，该方法就返回小于0的值
            //如果该实例应排在参数对象后面，该方法就返回大于0的值

            //按照LastName排序
            int result = string.Compare(this.LastName, other.LastName);
            //LastName如果相等的话，就比较FirstName
            if (result == 0)
            {
                result = string.Compare(this.FirstName, other.FirstName);
            }
            return result;
        }
    }
}
