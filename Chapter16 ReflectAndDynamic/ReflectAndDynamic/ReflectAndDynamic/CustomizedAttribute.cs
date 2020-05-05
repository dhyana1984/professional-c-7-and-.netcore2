using System;
namespace ReflectAndDynamic
{
    public class SocialSecurity
    {
        //编译器会在所有调用的命名空间下寻找FieldNameAttribute
        //FieldNameAttribute的构造函数的参数就是这个Attribute的必填参数
        //Comment可选参数是FieldNameAttribute的一个公共属性
        [FieldName("SocialSecurityNumber", Comment = "TestComment")]
        public string SocialSecurityNumber
        {
            get;set;
        }
    }


    //AttributeTargets只能应用到Attribute上，不能应用到类上，标识此Attribute可以应用到哪些类型的元素上，本例是应用到属性上
    //多个AttributeTargets只能应用到Attribute的话用|连接起来
    //AllowMultiple标识一个特性是否可以多次应用到同一项上
    //[FieldName("AA")]
    //[FieldName("BB")]
    //public class XXXXXX{}
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple =false, Inherited =false)]
    public class FieldNameAttribute : Attribute
    {
        private string _name;
        public FieldNameAttribute(string name)
        {
            _name = name;
        }
        public string Comment { get; set; }
    }
}
