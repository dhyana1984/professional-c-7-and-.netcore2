using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Person;

namespace ArraySample
{
    //定义用于PersonComparer的排序选项，使用FirstName或者FirstName排序
    public enum PersonCompareType
    {
        FirstName,
        LastName
    }

    //不能修改类中元素或者需要复杂逻辑排序时，可以使用IComparer实现类数组中的排序
    public class PersonComparer : IComparer<Person>
    {
        private PersonCompareType  _compareType;
        public PersonComparer(PersonCompareType compareType) =>
            _compareType = compareType;

        //IComparer是独立于要比较的类，所以Compare需要定义2个参数
        public int Compare(Person x, Person y)
        {
            if (x is null && y is null) return 0;
            if (x is null) return 1;
            if (y is null) return -1;
            switch(_compareType)
            {
                case PersonCompareType.FirstName:
                    return string.Compare(x.FirstName, y.FirstName);
                case PersonCompareType.LastName:
                    return string.Compare(x.LastName, y.LastName);
                default:
                    throw new ArgumentException("unexpected compare type");
            }
        }
    }
}
