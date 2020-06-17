﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Extentions;
using MVCSampleApp.Models;

namespace MVCSampleApp.Controllers
{
    public class HTMLHelpersController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HelperWithMenu() => View(GetSampleMenu());

        private Menu GetSampleMenu() =>
          new Menu
          {
              Id = 1,
              Text = "Schweinsbraten mit Knödel und Sauerkraut",
              Price = 6.9,
              Date = new DateTime(2017, 11, 14),
              Category = "Main"
          };

        public IActionResult HelperList()
        {
            var cars = new Dictionary<int, string>();
            cars.Add(1, "Red Bull Racing");
            cars.Add(2, "McLaren");
            cars.Add(3, "Mercedes");
            cars.Add(4, "Ferrari");
            //ToSelectListItems传入的参数是DropDownList选中的id
            return View(cars.ToSelectListItems(4));
        }

        public IActionResult StronglyTypedMenu() => View(GetSampleMenu());

    }
}
