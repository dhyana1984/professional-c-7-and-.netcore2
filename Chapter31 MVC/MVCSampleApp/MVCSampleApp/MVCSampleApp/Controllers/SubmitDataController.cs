using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Models;

namespace MVCSampleApp.Controllers
{
    public class SubmitDataController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult CreateMenu() => View();
        [HttpPost]
        public IActionResult CreateMenu(int id, string text, double price, DateTime date, string category)
        {
            var m = new Menu { Id = id, Text = text, Price = price, Date = date, Category = category };
            ViewBag.Info = $"menu created: {m.Text}, Price: {m.Price}, date: {m.Date.ToShortDateString()}, category: {m.Category}";
            return View("Index");
        }

        public IActionResult CreateMenu2() => View();
        [HttpPost]
        //模型绑定
        public IActionResult CreateMenu2(Menu menu)
        {
            ViewBag.Info = $"menu created: {menu.Text}, Price: {menu.Price}, date: {menu.Date.ToShortDateString()}, category: {menu.Category}";
            return View("Index");
        }

        public IActionResult CreateMenu3() => View();
        [HttpPost]
        //没有在Action传入参数
        public async Task<IActionResult> CreateMenu3Result()
        {
            var m = new Menu();
            //通过TryUpdateModelAsync方法检测Menu的实例是否被有效更新，此时m的各个属性值就是页面中传递过来的标签的值
            bool updated = await TryUpdateModelAsync<Menu>(m);
            if (updated)
            {
                ViewBag.Info = $"menu created: {m.Text}, Price: {m.Price}, date: {m.Date.ToShortDateString()}, category: {m.Category}";
                return View("Index");
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult CreateMenu4() => View();
        [HttpPost]
        public IActionResult CreateMenu4(Menu menu)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Info =
                  $"menu created: {menu.Text}, Price: {menu.Price}, date: {menu.Date.ToShortDateString()}, category: {menu.Category}";
            }
            else
            {
                ViewBag.Info = "not valid";
            }
            return View("Index");
        }
    }
}
