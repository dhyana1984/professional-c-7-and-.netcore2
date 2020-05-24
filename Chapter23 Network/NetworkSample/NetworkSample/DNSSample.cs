using System;
using System.Net;
using System.Threading.Tasks;

namespace NetworkSample
{
    public static class DNSSample
    {
        public static void DisplaySample()
        {

        }

        public static async Task OnLookupAsync(string hostname)
        {
            try
            {
                //通过GetHostEntryAsync得到IPHostEntry
                IPHostEntry ipHost = await Dns.GetHostEntryAsync(hostname);
                Console.WriteLine($"Hostname: {ipHost.HostName}");
                //在IPHostEntry中使用AddressList属性访问地址列表
                foreach (var item in ipHost.AddressList)
                {
                    Console.WriteLine($"Address Family: {item.AddressFamily}");
                    Console.WriteLine($"Address: {item}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
