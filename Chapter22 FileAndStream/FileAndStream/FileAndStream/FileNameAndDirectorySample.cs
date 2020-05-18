using System;
using System.IO;

namespace FileAndStream
{
    public static class FileNameAndDirectorySample
    {
        const string SampleFileName = "Sample1.md";
        public static void DisplaySample()
        {
            CreateFile();
        }
        static string GetDocumentFolder() =>
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        static void CreateFile()
        {
            string filename = Path.Combine(GetDocumentFolder(), SampleFileName);
            //创建文本并写入数据
            File.WriteAllText(filename, "Hello, World");
        }
    }
}
