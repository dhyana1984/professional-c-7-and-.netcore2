using System;
using System.Collections.Generic;
using System.IO;

namespace FileAndStream
{
    public static class ReadAndWriteFile
    {
        static string GetDocumentsFolder() =>
           Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        public static void Excute()
        {
            string fileName = Path.Combine(GetDocumentsFolder(), "movies.txt");
            WriteAFile();
            ReadingAFileLineByLine(fileName);
        }

        private static void WriteAFile()
        {
            string fileName = Path.Combine(GetDocumentsFolder(), "movies.txt");
            string[] movies =
            {
                "Snow White And The Seven Dwarfs",
                "Gone With The Wind",
                "Casablanca",
                "The Bridge On The River Kwai",
                "Some Like It Hot"
            };
            //WriteAllLines讲一个string数组的内容写到文件
            File.WriteAllLines(fileName, movies);

            string[] moreMovies =
            {
                "Psycho",
                "Easy Rider",
                "Star Wars",
                "The Matrix"
            };
            //AppendAllLines追加string数组格式的内容到文件
            File.AppendAllLines(fileName, moreMovies);
        }

        private static void ReadingAFileLineByLine(string fileName)
        {
            //一次性读取所有行到内存，返回字符串数组
            string[] lines = File.ReadAllLines(fileName);
            int i = 1;
            foreach (var line in lines)
            {
                Console.WriteLine($"{i++}. {line}");
            }

            //逐行读取，不需要等待所有行都读取完
            IEnumerable<string> lines2 = File.ReadLines(fileName);
            i = 1;
            foreach (var line in lines2)
            {
                Console.WriteLine($"{i++}. {line}");
            }
        }
    }
}
