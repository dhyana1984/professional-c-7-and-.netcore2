using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSample
{
    public static class TCPSample
    {

        private const int ReadBifferSize = 1024;
        public static async Task<string> RequestHtmlAsync(string hostname)
        {
            try
            {
                //实例化TcpClient对象
                //TCPClient封装了TCP连接
                using (var client= new TcpClient())
                {
                    //在80端口上建立到主机的TCP连接
                    await client.ConnectAsync(hostname, 80);
                    //检索一个流，使用这个连接进行读写，流可以用来把请求写到服务器，读取响应
                    NetworkStream stream = client.GetStream();
                    //设置header为http请求
                    string header = "GET / HTTP/1.1\r\n" +
                     $"Host: {hostname}:80\r\n" +
                     "Connection: close\r\n" +
                     //结束header信息
                     "\r\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(header);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    //立即向服务器发送，避免缓存在本地
                    await stream.FlushAsync();

                    //接受服务器响应信息
                    //因为不知道响应的数据有多大，所以用MemoryStream
                    var ms = new MemoryStream();
                    buffer = new byte[ReadBifferSize];
                    int read = 0;
                    do
                    {
                        read = await stream.ReadAsync(buffer, 0, ReadBifferSize);
                        //把流中返回的信息写入MemoryStream
                        ms.Write(buffer, 0, read);
                        Array.Clear(buffer, 0, buffer.Length);
                    } while (read > 0);
                    ms.Seek(0, SeekOrigin.Begin);
                    //从服务器中读取所有数据后，SteamReader接管控制，把数据从流读入一个字符串并返回
                    using (var reader = new StreamReader(ms))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
