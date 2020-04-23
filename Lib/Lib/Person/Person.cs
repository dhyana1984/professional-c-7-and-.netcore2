using System;
namespace Lib.Person
{
    public class Person
    {
        public DateTime Birthday { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Person(string firstName, string lastName, DateTime birthday)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public override string ToString() => $"{FirstName} {LastName}";
    }

}
