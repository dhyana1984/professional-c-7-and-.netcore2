using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace FileAndStream
{
    public class PipeLineReader
    {
        public PipeLineReader(string pipeName)
        {
           
            if (pipeName == "anon")
            {
                AnonymousReader();
            }
            else
            {
                PipesReader(pipeName);
            }
        }
        //命名管道服务端，读取消息
        private  void PipesReader(string pipeName)
        {
            try
            {
                //NamedPipeServerStream创建一个服务，派生自Steam可以使用Stream的所有功能
                //PipeDirection.In 单向，用于读取
                //using (var pipeReader = new NamedPipeServerStream(pipeName, PipeDirection.In))
                //{
                //    //命名管道等待写入方的连接
                //    pipeReader.WaitForConnection();
                //    Console.WriteLine("reader connected");
                //    const int BUFFERSIZE = 256;

                //    bool completed = false;
                //    while (!completed)
                //    {
                //        byte[] buffer = new byte[BUFFERSIZE];
                //        //读取PipeLine的内容
                //        int nRead = pipeReader.Read(buffer, 0, BUFFERSIZE);
                //        string line = Encoding.UTF8.GetString(buffer, 0, nRead);
                //        Console.WriteLine(line);
                //        if (line == "bye") completed = true;
                //    }
                //}

                var pipeReader = new NamedPipeServerStream(pipeName, PipeDirection.In);
                //使用Stream读取PipeLine，简化一些
                using (var reader = new StreamReader(pipeReader))
                {
                    pipeReader.WaitForConnection();
                    Console.WriteLine("reader connected");

                    bool completed = false;
                    while (!completed)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine(line);
                        if (line == "bye") completed = true;
                    }
                }
                Console.WriteLine("completed reading");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //匿名管道服务端
        private void AnonymousReader()
        {
            using (var reader = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable))
            {
                Console.WriteLine("using anonymous pipe");
                //需要知道客户端的句柄，这个句柄在GetClientHandleAsString方法中转换为一个字符串
                //这个变量以后由充当写入器的客户端使用
                string pipeHandle = reader.GetClientHandleAsString();
                Console.WriteLine($"pipe handle: {pipeHandle}");

                byte[] buffer = new byte[256];
                int nRead = reader.Read(buffer, 0, 256);

                string line = Encoding.UTF8.GetString(buffer, 0, 256);
                Console.WriteLine(line);
            }
        }
    }
}

