using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCSampleApp.Controllers
{
    public class TagHelperController : Controller
    {
        public IActionResult CustomTable() => View(GetSampleMenus());
        private IList<Menu> GetSampleMenus() =>
            new List<Menu>
            {
                new Menu
                {
                    Id = 1,
                    Text = "Schweinsbraten mit Knödel und Sauerkraut",
                    Price = 8.5,
                    Date = new DateTime(2018, 10, 5),
                    Category = "Main"
                },
                new Menu
                {
                    Id = 2,
                    Text = "Erdäpfelgulasch mit Tofu und Gebäck",
                    Price = 8.5,
                    Date = new DateTime(2018, 10, 6),
                    Category = "Vegetarian"
                },
                new Menu
                {
                    Id = 3,
                    Text = "Tiroler Bauerngröst'l mit Spiegelei und Krautsalat",
                    Price = 8.5,
                    Date = new DateTime(2018, 10, 7),
                    Category = "Vegetarian"
                }
            };
    }
}
