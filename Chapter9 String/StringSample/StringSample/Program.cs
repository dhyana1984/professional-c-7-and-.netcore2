using System;
using System.Text.RegularExpressions;
using Lib.Person;

namespace StringSample
{

    class Program
    {
        const string text =
            @"Professional C# 6 and .NET Core 1.0 provides complete coverage " +
            "of the latest updates, features, and capabilities, giving you " +
            "everything you need for C#. Get expert instruction on the latest " +
            "changes to Visual Studio 2015, Windows Runtime, ADO.NET, ASP.NET, " +
            "Windows Store Apps, Windows Workflow Foundation, and more, with " +
            "clear explanations, no-nonsense pacing, and valuable expert insight. " +
            "This incredibly useful guide serves as both tutorial and desk " +
            "reference, providing a professional-level review of C# architecture " +
            "and its application in a number of areas. You'll gain a solid " +
            "background in managed code and .NET constructs within the context of " +
            "the 2015 release, so you can get acclimated quickly and get back to work.";
        static void Main(string[] args)
        {
            var day = new DateTime(2025, 2, 14);
            Console.WriteLine($"{day:d}");          //02/14/2025
            Console.WriteLine($"{day:dd-MM-yyyy}"); //14-02-2025

            double d = 3.1415;
            //#格式说明符是数字占位符，如果数字可用，就显示数字，如果数字不可用，就不显示数字
            Console.WriteLine($"{d:###.###}");  //3.142
            //0格式说明符是一个零占位符，显示相应的数字，如果数字不存在，就显示0
            Console.WriteLine($"{d:000.000}");  //0003.142
            Console.WriteLine("-------------");

            var p1 = new FormatPerson("Stephanie", "Nagel", new DateTime(1984, 1, 2));
            Console.WriteLine(p1.ToString("F"));
            Console.WriteLine($"{p1:L}");
            Console.WriteLine("-------------");

            Find1(text);
            Console.WriteLine("-------------");
            Find2(text);
            Console.WriteLine("-------------");
            NamedGroups();
        }

        public static void Find1(string text)
        {
            const string pattern = "ion";
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            WriteMatches(text, matches);
        }

        public static void Find2(string text)
        {
            //\b是表示边界，\ba表示a开头，ure\b表示ure结尾
            //\S表示任意不为空格的字符，*是通配符
            string pattern = @"\ba\S*ure\b";
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            WriteMatches(text, matches);
        }

        public static void WriteMatches(string text, MatchCollection matches)
        {
            Console.WriteLine($"Original text was: \n\n{text}\n");
            Console.WriteLine($"No. of matches: {matches.Count}");

            foreach (Match nextMatch in matches)
            {
                int index = nextMatch.Index;
                string result = nextMatch.ToString();
                int charsBefore = (index < 5) ? index : 5;
                int fromEnd = text.Length - index - result.Length;
                int charsAfter = (fromEnd < 5) ? fromEnd : 5;
                int charsToDisplay = charsBefore + charsAfter + result.Length;

                Console.WriteLine($"Index: {index}, \tString: {result}, \t" +
                  $"{text.Substring(index - charsBefore, charsToDisplay)}");
            }
        }

        public static void NamedGroups()
        {
            Console.WriteLine("NamedGroups\n");
            string line = "Hey, I've just found this amazing URI at http:// what was it --oh yes https://www.wrox.com or http://www.wrox.com:80";
            //正则表达式分组，使用()小括号
            //使用?<groupname>可以给分组命名，用?:来忽略组
            string pattern = @"\b(?<protocol>https?)(?:://)(?<address>[.\w]+)([\s:](?<port>[\d]{2,4})?)\b";
                        
            var r = new Regex(pattern, RegexOptions.ExplicitCapture);
            MatchCollection mc = r.Matches(line);
            foreach (Match m in mc)
            {
                //找到每个匹配结果
                foreach (Group g in m.Groups)
                {
                    //找到每个匹配结果的每个分组的内容
                    if(g.Success)
                    {
                       
                        Console.WriteLine($"group index: {g.Index}, value: {g.Value}");
                    }
                }

            }
            Console.WriteLine("-------------");
            foreach (Match m in mc)
            {
                Console.WriteLine($"match: {m} at {m.Index}");
                //通过分组名称遍历
                foreach (var groupName in r.GetGroupNames())
                {
                    Console.WriteLine($"match for {groupName}: {m.Groups[groupName].Value}");
                }

            }

        }
    }
}
