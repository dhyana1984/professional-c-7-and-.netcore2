using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Server;

namespace NetworkSample
{
    public static class WebListenerSample
    {

        public static async Task StartServerAsync(params string[] prefixes)
        {
            try
            {
                Console.WriteLine($"server starting at");
                var listener = new WebListener();

                foreach (var prefix in prefixes)
                {
                    listener.Settings.UrlPrefixes.Add(prefix);
                    Console.WriteLine($"\t{prefix}");
                }

                listener.Start();

                do
                {
                    //listener.AcceptAsync()监听访问Server的请求
                    using (RequestContext context = await listener.AcceptAsync())
                    {
                        context.Response.Headers.Add("content-type", new string[] { "text/html" });
                        context.Response.StatusCode = (int)HttpStatusCode.OK;

                        byte[] buffer = GetHtmlContent(context.Request);
                        await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                    }

                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ShowUsage() =>
    Console.WriteLine("Usage: HttpServer Prefix [Prefix2] [Prefix3] [Prefix4]");

        private static string s_htmlFormat =
            "<!DOCTYPE html><html><head><title>{0}</title></head>" +
            "<body>{1}</body></html>";


        private static byte[] GetHtmlContent(Request request)
        {
            string title = "Sample WebListener";

            string content = $"<h1>Hello from the server</h1>" +
                $"<h2>Header Info</h2>" +
                $"{string.Join(' ', GetHeaderInfo(request.Headers))}" +
                $"<h2>Request Object Information</h2>" +
                $"{string.Join(' ', GetRequestInfo(request))}";

            string html = string.Format(s_htmlFormat, title, content);
            return Encoding.UTF8.GetBytes(html);
        }

        private static IEnumerable<string> GetRequestInfo(Request request) =>
            request.GetType().GetProperties().Select(p => $"<div>{p.Name}: {p.GetValue(request)}</div>");

        private static IEnumerable<string> GetHeaderInfo(HeaderCollection headers) =>
            headers.Keys.Select(key => $"<div>{key}: {string.Join(",", headers.GetValues(key))}</div>");

    }

}
