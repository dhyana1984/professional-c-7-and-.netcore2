using System;
using System.Threading.Tasks;

namespace MenuSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new MenusContext())
            {
                bool created = await context.Database.EnsureCreatedAsync();
                string creationInfo = created ? "created" : "existed";
                Console.WriteLine($"database {creationInfo}");
            }
        }
    }
}
