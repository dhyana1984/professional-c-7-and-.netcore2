using System;
using System.Net;

namespace NetworkSample
{
    public static class IPAddressSample
    {
        public static void DisplaySample(string ipAddressString)
        {
            IPAddress address;
            if (!IPAddress.TryParse(ipAddressString, out address))
            {
                Console.WriteLine($"cannot parse {ipAddressString}");
                return;
            }
            //GetAddressBytes把地址本身作为字节数组
            byte[] bytes = address.GetAddressBytes();
            for (int i = 0; i < bytes.Length; i++)
            {
                Console.WriteLine($"byte {i} : {bytes[i] :X}");
            }
            Console.WriteLine($"family: {address.AddressFamily}, map to ipv6: {address.MapToIPv6()}, map to ipv4: {address.MapToIPv4()}");
        }
    }
}
