using System;

namespace FileAndStream
{
    class Program
    {
        static void Main(string[] args)
        {
            //FileNameAndDirectorySample.DisplaySample();
            FileWatcherSample.WatchFiles("/Users/yxiong/Desktop", "*.md");
            Console.ReadLine();
        }
    }
}
