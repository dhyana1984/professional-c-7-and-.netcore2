using System;
using System.Threading.Tasks;

namespace MenuSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await DeleteDatabase();
            await CreateDatabase();
        }

        private static async Task DeleteDatabase()
        {
            Console.Write("Delete the database? ");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using (var context = new MenusContext())
                {
                    await context.Database.EnsureDeletedAsync();
                }
            }
        }


        private static async Task CreateDatabase()
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
