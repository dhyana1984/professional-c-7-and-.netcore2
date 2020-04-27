using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Racer;

namespace Collections
{
    public class LookupSample
    {
        public void DisplaySample()
        {
            var racers = new List<Racer>();
            racers.Add(new Racer(26, "Jacques", "Villeneuve", "Canada", 11));
            racers.Add(new Racer(18, "Alan", "Jones", "Australia", 12));
            racers.Add(new Racer(11, "Jackie", "Stewart", "United Kingdom", 27));
            racers.Add(new Racer(15, "James", "Hunt", "United Kingdom", 10));
            racers.Add(new Racer(5, "Jack", "Brabham", "Australia", 14));
            //Lookup可以把键映射到一个集合，即相比Dictionary是一对一，Lookup是一对多，
            var lookupRacers = racers.ToLookup(r => r.Country);
            foreach (var r in lookupRacers["Australia"])
            {
                Console.WriteLine(r);
            }
        }
    }
}
