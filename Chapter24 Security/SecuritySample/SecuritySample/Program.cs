using System;
using System.Threading.Tasks;

namespace SecuritySample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //PrincipalSample.DisplaySample();
            //Console.WriteLine("---------------");
            //SigningDemo.DisplaySample();
            //Console.WriteLine("---------------");
            //await SecureTransfer.DisplaySample();
            //Console.WriteLine("---------------");
            RSASignature.DisplaySample();
        }
    }
}
