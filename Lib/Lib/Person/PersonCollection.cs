using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Person
{

    public class PersonCollection
    {
        private Person[] _people;
        public PersonCollection(params Person[] people) =>
            _people = people.ToArray();

        //索引器，类似属性，但是要用this关键字，并且在中括号定义索引类型
        public Person this[int index]
        {
            get => _people[index];
            set => _people[index] = value;
        }
        //通过DateTime类型作为索引，并且返回Person集合
        public IEnumerable<Person> this[DateTime birthday] =>
            _people.Where(p => p.Birthday == birthday);


    }
}
