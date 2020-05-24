using System;
using System.Threading.Tasks;
using NetWorkSample;

namespace NetworkSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //HttpClientSample.DisplaySample();
            //Console.ReadLine();
            //IPAddressSample.DisplaySample("202.76.223.13");
            //await DNSSample.OnLookupAsync("www.baidu.com");
            //var res = await TCPSample.RequestHtmlAsync("www.baidu.com");
            //Console.WriteLine(res);
            //Console.ReadLine();
            TCPServer.Run();
        }
    }
}
