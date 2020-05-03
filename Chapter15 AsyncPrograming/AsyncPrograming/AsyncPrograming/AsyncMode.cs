using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AsyncPrograming
{
    public static class AsyncMode
    {
        const string url = "https://www.baidu.com";
        //Async Mode
        public static void AsynchronousPattern()
        {
            Console.WriteLine(nameof(AsynchronousPattern));
            WebRequest request = WebRequest.Create(url);
            IAsyncResult result = request.BeginGetResponse(ReadResponse, null);
            //此回调没有在UI线程中运行
            void ReadResponse(IAsyncResult ar)
            {
                using (WebResponse response = request.EndGetResponse(ar))
                {
                    Stream stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content.Substring(0,100));
                    Console.WriteLine();
                }
                
            }

        }

        //Event-based Asynchronous
        public static void EventBasedAsyncPattern()
        {
            Console.WriteLine(nameof(EventBasedAsyncPattern));
            using(var client = new WebClient())
            {
                //先定义DownloadStringCompleted事件
                //事件处理程序将通过保存同步上下文的线程来调用，所以可以直接从事件处理程序中访问UI元素
                client.DownloadStringCompleted += (sender, e) =>
                {
                    Console.WriteLine(e.Result.Substring(0, 100));
                };
                //后调用DownloadStringAsync
                client.DownloadStringAsync(new Uri(url));
                Console.WriteLine();
            }
        }

        //TAP mode
        public static async Task TaskBasedAsynchronousPatternAsync()
        {
            Console.WriteLine(nameof(TaskBasedAsynchronousPatternAsync));
            using(var client = new WebClient())
            {
                string content = await client.DownloadStringTaskAsync(url);
                Console.WriteLine(content.Substring(0, 100));
            }
        }



    }
}
