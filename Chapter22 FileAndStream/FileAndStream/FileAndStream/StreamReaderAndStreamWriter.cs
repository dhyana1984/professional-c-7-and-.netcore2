using System;
using System.IO;
using System.Text;

namespace FileAndStream
{
    public static class StreamReaderAndStreamWriter
    {

        static void ReadFileUsingReader(string filename)
        {
            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            //StreamReader默认使用utf-8编码
            //var reader = File.OpenText(filename) 也可以创建SteamReader
            using(var reader = new StreamReader(stream)) //FileStream传入StreamReader创建StreamReader对象
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Console.WriteLine(line);
                }
            }
        }

        public static void WriteFileUsingWriter(string fileName, string[] lines)
        {
            var outputStream = File.OpenWrite(fileName);
            //StreamWriter默认使用utf-8编码
            using (var writer = new StreamWriter(outputStream))
            {
                byte[] preamble = Encoding.UTF8.GetPreamble();
                outputStream.Write(preamble, 0, preamble.Length);
                writer.Write(lines);
            }
        }
    }
}
