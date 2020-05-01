using System;
namespace FunctionalPrograming
{
    public class PersonSample
    {
        public PersonSample(string name) =>
            name.Split(' ').ToStrings(out _firstName, out _lastName);

        private string _firstName;
        private string _lastName;

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }

        public override string ToString() =>
           $"FirstName: {FirstName} LastName: {LastName}";

        //定义解构方法, 将分离的部分放入out参数中
        //该方法总是返回void，并用out参数返回各部分
        //可以使用扩展方法来实现Deconstruct
        public void Deconstruct(out string firstName, out string lastName)
        {
            firstName = _firstName;
            lastName = _lastName;
        }
    }
}
