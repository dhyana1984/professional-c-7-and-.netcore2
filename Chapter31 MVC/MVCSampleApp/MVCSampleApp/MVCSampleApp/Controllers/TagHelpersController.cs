using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Models;

namespace MVCSampleApp.Controllers
{
    public class TagHelpersController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LabelHelper() => View(GetSampleMenu());
        private Menu GetSampleMenu() =>
          new Menu
          {
              Id = 1,
              Text = "Schweinsbraten mit Knödel und Sauerkraut",
              Price = 6.9,
              Date = new DateTime(2018, 10, 5),
              Category = "Main"
          };

        public IActionResult InputHelper() => View(GetSampleMenu());

        public IActionResult FormHelper() => View(GetSampleMenu());

        [HttpPost]
        public IActionResult FormHelper(Menu m)
        {
            if (!ModelState.IsValid)
            {
                //如果验证不通过，显示同样的视图
                return View(m);
            }
            return View("ValidationHelperResult", m);
        }

        public IActionResult Markdown() => View();
    }
}
