using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WhatsNewAttributes;

namespace ReflectAndDynamic
{
    class Program
    {
        private static StringBuilder outputText = new StringBuilder();
        private static DateTime backDateTo = new DateTime(2017, 2, 11);
        static void Main(string[] args)
        {
            //获得double的Type
            //Type t = typeof(double);
            //AnalyzeType(t);
            //Console.WriteLine($"Analysis of type {t.Name}");
            //Console.WriteLine(outputText.ToString());



            //Console.ReadLine();

            Assembly theAssembly = Assembly.Load(new AssemblyName("VectorClass"));
            Attribute supportsAttribute = theAssembly.GetCustomAttribute(typeof(SupportsWhatsNewAttribute));
            AddToOutput($"Assembly: {theAssembly.FullName}");
            if (supportsAttribute == null)
            {
                AddToOutput("This assembly does not support WhatsNew attributes");
                return;
            }
            else
            {
                AddToOutput("Defined Types:");
            }

            IEnumerable<Type> types = theAssembly.ExportedTypes;

            foreach (Type definedType in types)
            {
                DisplayTypeInfo(definedType);
            }
            Console.WriteLine($"What\'s New since {backDateTo:D}");
            Console.WriteLine(outputText.ToString());

            Console.ReadLine();


        }

        private static void DisplayTypeInfo(Type type)
        {
            if (!type.GetTypeInfo().IsClass)
            {
                return;
            }
            AddToOutput($"{Environment.NewLine}class {type.Name}");

            IEnumerable<LastModifiedAttribute> lastModifiedAttributes = type.GetTypeInfo()
                                                                            .GetCustomAttributes()
                                                                            .OfType<LastModifiedAttribute>()
                                                                            .Where(t => t.DateModified >= backDateTo)
                                                                            .ToArray();

            if (lastModifiedAttributes.Count() == 0)
            {
                AddToOutput($"\tNo changes to the class {type.Name}{Environment.NewLine}");
            }
            else
            {
                foreach (LastModifiedAttribute attribute in lastModifiedAttributes)
                {
                    WriteAttributeInfo(attribute);
                }
            }

            AddToOutput("changes to methods of this class:");

            //使用TypeInfo类型的DeclaredMembers属性遍历这种数据类型的所有成员方法
            foreach (MethodInfo method in type.GetTypeInfo().DeclaredMembers.OfType<MethodInfo>())
            {
                IEnumerable<LastModifiedAttribute> attributesToMethods = method.GetCustomAttributes()
                    .OfType<LastModifiedAttribute>().Where(a => a.DateModified >= backDateTo).ToArray();

                if (attributesToMethods.Count() > 0)
                {
                    AddToOutput($"{method.ReturnType} {method.Name}()");

                    foreach (Attribute attribute in attributesToMethods)
                    {
                        WriteAttributeInfo(attribute);
                    }
                }
            }
        }

        private static void WriteAttributeInfo(Attribute attribute)
        {
            if (attribute is LastModifiedAttribute lastModifiedAttribute)
            {
                AddToOutput($"\tmodified: {lastModifiedAttribute.DateModified:D}: {lastModifiedAttribute.Changes}");

                if (lastModifiedAttribute.Issues != null)
                {
                    AddToOutput($"\tOutstanding issues: {lastModifiedAttribute.Issues}");
                }
            }
        }

        static void AnalyzeType(Type t)
        {
            TypeInfo typeInfo = t.GetTypeInfo();
            //获得Type基本信息
            AddToOutput($"Type Name: {t.Name}");
            AddToOutput($"Full Name: {t.FullName}");
            AddToOutput($"Namespace: {t.Namespace}");

            //获得基类
            Type tBase = typeInfo.BaseType;
            if(tBase != null)
            {
                AddToOutput($"Base Type: {tBase.Name}");
            }

            AddToOutput("\npublic members:");

            //遍历所有成员
            foreach (MemberInfo member in t.GetMembers())
            {
                AddToOutput($"{member.DeclaringType} {member.MemberType} {member.Name}");
            }
        }

        static void AddToOutput(string Text) =>
            outputText.Append($"{Environment.NewLine} {Text}");
    }
}
