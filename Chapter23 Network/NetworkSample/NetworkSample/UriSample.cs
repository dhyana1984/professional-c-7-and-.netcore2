using System;
namespace NetworkSample
{
    public static class UriSample
    {
        public static void DisplaySample(string url)
        {
            //使用Uri可以分析，组合和比较URI
            //这些类型都是只读属性
            var page = new Uri(url);
            Console.WriteLine($"scheme: {page.Scheme}");
            Console.WriteLine($"host: {page.Host}, type: {page.HostNameType}, idn host: {page.IdnHost}");
            Console.WriteLine($"port: {page.Port}");
            Console.WriteLine($"path: {page.AbsolutePath}");
            Console.WriteLine($"query: {page.Query}");

            foreach (var segment in page.Segments)
            {
                Console.WriteLine($"segment: {segment}");
            }

            //UriBuilder允许把给定的字符串当做URI的组成部分构建成一个URI
            var builder = new UriBuilder();
            builder.Host = "www.cninnovation.com";
            builder.Port = 80;
            builder.Path = "traning/MVC";
            Uri uri = builder.Uri;
            Console.WriteLine(uri);
        }
    }
}
