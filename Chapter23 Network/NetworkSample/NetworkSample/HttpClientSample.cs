using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NetWorkSample
{
    public static class HttpClientSample
    {
        public async static void DisplaySample()
        {
            //await GetDataSimpleAsync();
            //Console.WriteLine("-------------");
            //await GetDataWithExceptionsAsync();
            await GetDataWithHeadersAsync(); 
        }

        private const string NorthwindUrl = "http://services.odata.org/Northwind/Northwind.svc/Regions";
        private const string IncorrectUrl = "http://services.odata.org/Northwind1/Northwind.svc/Regions";

        //HttpClient虽然实现了IDisposable接口，但是HttpClient的Dispose方法不会立即释放相关套接字，而是超时后释放，所以可能导致耗尽套接字所以需要重用
        //实现_httpClient的重用,不是每次都实例化一个HttpClient新对象
        //HttpClient是线程安全的，所以可以用于处理多个请求。每个HttpClient实例都维护自己的线程池，HttpClient实例之间的请求会被隔离
        private static HttpClient _httpClient;
        public static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        private static async Task GetDataSimpleAsync()
        {
            //返回调用用线程，执行其他工作，GetAsync结果可用时，就用该方法继续线程，响应写入response变量
            HttpResponseMessage response = await HttpClient.GetAsync(NorthwindUrl);
            //IsSuccessStatusCode属性确定请求是否成功
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Received payload of {responseBodyAsText.Length} characters");
                Console.WriteLine();
                Console.WriteLine(responseBodyAsText);
            }
        }

        private static async Task GetDataWithExceptionsAsync()
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                ShowHeaders("Request Headers:", HttpClient.DefaultRequestHeaders);
                HttpResponseMessage response = await HttpClient.GetAsync(IncorrectUrl);
                //GetAsync默认情况下不产生异常
                //调用EnsureSuccessStatusCode会返回HttpResponseMessage，该方法检查是否为false，如果是就抛出异常
                response.EnsureSuccessStatusCode();

                ShowHeaders("Response Headers:", response.Headers);

                Console.WriteLine($"Response Status Code: {response.StatusCode} {response.ReasonPhrase}");
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Received payload of {responseBodyAsText.Length} characters");
                Console.WriteLine();
                Console.WriteLine(responseBodyAsText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }


        private static void ShowHeaders(string title, HttpHeaders headers)
        {
            Console.WriteLine(title);
            foreach (var header in headers)
            {
                string value = string.Join(" ", header.Value);
                Console.WriteLine($"Header: {header.Key} Value: {value}");
            }
            Console.WriteLine();
        }

        private static async Task GetDataWithHeadersAsync()
        {
            try
            {
                //DefaultRequestHeaders属性允许设置或者改变Header，使用Add方法可以给集合添加Header
                HttpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                //可以读取header值，但是是只读的
                ShowHeaders("Request Headers:", HttpClient.DefaultRequestHeaders);

                HttpResponseMessage response = await HttpClient.GetAsync(NorthwindUrl);
                response.EnsureSuccessStatusCode();

                ShowHeaders("Response Headers:", response.Headers);

                Console.WriteLine($"Response Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Received payload of {responseBodyAsText.Length} characters");
                Console.WriteLine();
                Console.WriteLine(responseBodyAsText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        //自定义请求
        public class SampleMessageHandler : HttpClientHandler
        {
            private string _displayMessage;
            public SampleMessageHandler(string message) => _displayMessage = message;

            //通过继承HttpClientHandler重写SendAsync来自定义请求
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"In SampleMessageHandler {_displayMessage}");
                if (_displayMessage == "error")
                {
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    return Task.FromResult<HttpResponseMessage>(response);
                }

                return base.SendAsync(request, cancellationToken);
            }
        }

    }
}
