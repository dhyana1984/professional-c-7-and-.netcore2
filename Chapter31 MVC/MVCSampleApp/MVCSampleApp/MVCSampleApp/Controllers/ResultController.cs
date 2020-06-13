using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Models;

namespace MVCSampleApp.Controllers
{
    public class ResultController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JsonDemo()
        {
            var m = new Menu
            {
                Id = 3,
                Text = "Grilled sausage with sauerkraut and potatoes",
                Price = 12.90,
                Date = new DateTime(2018, 3, 31),
                Category = "Main"
            };
            return Json(m);
        }

        //返回一个VirtualFileResult，显示在页面上
        public IActionResult FileDemo() =>
            File("~/images/timg.jpeg", "image/jpeg");
    }
}
