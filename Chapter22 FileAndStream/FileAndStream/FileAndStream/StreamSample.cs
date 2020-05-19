using System;
using System.IO;
using System.Text;

namespace FileAndStream
{
    public static class StreamSample
    {
        static void DisplaySample()
        {

        }

        // read BOM
        // BOM是字节顺序标记，也叫做序言，提供了文件如何编码的信息
        public static Encoding GetEncoding(Stream stream)
        {
            if (!stream.CanSeek) throw new ArgumentException("require a stream that can seek");

            Encoding encoding = Encoding.ASCII;

            
            byte[] bom = new byte[5];
            //读取从0开始5个字节的数组
            int nRead = stream.Read(bom, offset: 0, count: 5);
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
            {
                Console.WriteLine("UTF-32");
                stream.Seek(4, SeekOrigin.Begin);
                return Encoding.UTF32;
            }
            else if (bom[0] == 0xff && bom[1] == 0xfe)
            {
                Console.WriteLine("UTF-16, little endian");
                stream.Seek(2, SeekOrigin.Begin);
                return Encoding.Unicode;
            }
            else if (bom[0] == 0xfe && bom[1] == 0xff)
            {
                Console.WriteLine("UTF-16, big endian");
                stream.Seek(2, SeekOrigin.Begin);
                return Encoding.BigEndianUnicode;
            }
            else if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                Console.WriteLine("UTF-8");
                stream.Seek(3, SeekOrigin.Begin);
                return Encoding.UTF8;
            }
            stream.Seek(0, SeekOrigin.Begin);
            return encoding;
        }

        //读取流
        public static void ReadFileUsingFileStream(string fileName)
        {
            const int BUFFERSIZE = 4096;
            //创建一个读取流
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //显示文件属性信息
                ShowStreamInformation(stream);
                //获取编码
                Encoding encoding = GetEncoding(stream);


                byte[] buffer = new byte[BUFFERSIZE];

                bool completed = false;
                do
                {
                    //使用Read方法读取文件，
                    int nread = stream.Read(buffer, 0, BUFFERSIZE);
                    //直到返回0
                    if (nread == 0) completed = true;
                    if (nread < BUFFERSIZE)
                    {
                        Array.Clear(buffer, nread, BUFFERSIZE - nread);
                    }

                    string s = encoding.GetString(buffer, 0, nread);
                    Console.WriteLine($"read {nread} bytes");
                    Console.WriteLine(s);
                } while (!completed);
            }
        }

        //写入流
        public static void WriteTextFile()
        {
            string tempTextFileName = Path.ChangeExtension(Path.GetTempFileName(), "txt");
            //创建一个可以写入的流
            using (FileStream stream = File.OpenWrite(tempTextFileName))
            {
                //// write BOM
                //stream.WriteByte(0xef);
                //stream.WriteByte(0xbb);
                //stream.WriteByte(0xbf);

                //GetPreamble()返回一个字节数组，包含文件的序言
                byte[] preamble = Encoding.UTF8.GetPreamble();
                //将文件序言用Stream的Write方法写入
                stream.Write(preamble, 0, preamble.Length);

                string hello = "Hello, World!";
                //GetBytes()方法将字符串转为UTF-8的字节数组
                byte[] buffer = Encoding.UTF8.GetBytes(hello);
                //写入流
                stream.Write(buffer, 0, buffer.Length);
                Console.WriteLine($"file {stream.Name} written");
            }
        }

        //复制流
        public static void CopyUsingStreams(string inputFile, string outputFile)
        {
            const int BUFFERSIZE = 4096;
            //合并读写流
            using (var inputStream = File.OpenRead(inputFile))
            using (var outputStream = File.OpenWrite(outputFile))
            {
                byte[] buffer = new byte[BUFFERSIZE];
                bool completed = false;
                do
                {
                    //从文件读取流
                    int nRead = inputStream.Read(buffer, 0, BUFFERSIZE);
                    if (nRead == 0) completed = true;
                    //写入另一个文件流
                    outputStream.Write(buffer, 0, nRead);
                } while (!completed);
            }
        }

        public static void ShowStreamInformation(Stream stream)
        {
            Console.WriteLine($"stream can read: {stream.CanRead}, can write: {stream.CanWrite}, can seek: {stream.CanSeek}, can timeout: {stream.CanTimeout}");
            Console.WriteLine($"length: {stream.Length}, position: {stream.Position}");
            if (stream.CanTimeout)
            {
                Console.WriteLine($"read timeout: {stream.ReadTimeout} write timeout: {stream.WriteTimeout} ");
            }
        }
    }
}
