using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace FileAndStream
{
    public class PipeLineWriter
    {
        public PipeLineWriter(string serverName, string pipeName)
        {
            string _pipeName = pipeName.Length >= 1 ? pipeName : "SamplePipe";
            string _serverName = serverName.Length >= 2 ? serverName : "localhost"; 
            if (pipeName == "anon")
            {
                AnonymousWriter();
            }
            else
            {
                PipesWriter(_serverName, _pipeName);
            }
        }

        //命名管道客户端，写入消息
        private static void PipesWriter(string serverName, string pipeName)
        {
            try
            {
                //需要服务名称，管道名称和管道方向
                //using (var pipeWriter = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out))
                //{
                //    //连接服务端
                //    pipeWriter.Connect();
                //    Console.WriteLine("writer connected");

                //    bool completed = false;
                //    //写入消息
                //    while (!completed)
                //    {
                //        string input = Console.ReadLine();
                //        if (input == "bye") completed = true;

                //        byte[] buffer = Encoding.UTF8.GetBytes(input);
                //        pipeWriter.Write(buffer, 0, buffer.Length);
                //    }
                //}
                var pipeWriter = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
                using (var writer = new StreamWriter(pipeWriter))
                {
                    pipeWriter.Connect();
                    Console.WriteLine("writer connected");

                    bool completed = false;
                    while (!completed)
                    {
                        string input = Console.ReadLine();
                        if (input == "bye") completed = true;

                        writer.WriteLine(input);
                        //不写入缓存，直接推到服务器
                        writer.Flush();
                    }
                }
                Console.WriteLine("completed writing");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //匿名管道客户端
        private void AnonymousWriter()
        {
            Console.WriteLine("using anonymous pipe");
            Console.Write("pipe handle: ");
            string pipeHandle = Console.ReadLine();
            using (var pipeWriter = new AnonymousPipeClientStream(PipeDirection.Out, pipeHandle))
            using (var writer = new StreamWriter(pipeWriter))
            {
                for (int i = 0; i < 100; i++)
                {
                    writer.WriteLine($"Message {i}");
                    Task.Delay(500).Wait();
                }
            }
        }
    }
}
