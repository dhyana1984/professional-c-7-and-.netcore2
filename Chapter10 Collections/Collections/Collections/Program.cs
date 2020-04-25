using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Racer;

namespace Collections
{
    class Program
    {
        static  async Task Main(string[] args)
        {
            ListSample listSample = new ListSample();
            listSample.DisplaySample();
            Console.WriteLine("------Dictionary-----");
            DictionarySample dictionarySample = new DictionarySample();
            dictionarySample.DisplaySample();
            Console.WriteLine("------Lookup-----");
            LookupSample lookupSample = new LookupSample();
            lookupSample.DisplaySample();
            Console.WriteLine("------Queue-----");
            QueueSample queueSample = new QueueSample();
            await queueSample.DisplaySample();
        }
    }
}
