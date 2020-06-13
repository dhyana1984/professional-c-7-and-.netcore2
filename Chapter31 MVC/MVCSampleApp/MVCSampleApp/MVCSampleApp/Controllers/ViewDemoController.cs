using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCSampleApp.Models;

namespace MVCSampleApp.Controllers
{
    public class ViewsDemoController : Controller
    {
        private EventsAndMenusContext _context;
        public ViewsDemoController(EventsAndMenusContext context)
        {
            _context = context;
        }
       
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LayoutSample() =>
          View();

        public IActionResult LayoutUsingSections() =>
            View();

        public IActionResult UseAPartialView1() =>
            View(_context);

        public IActionResult UseAPartialView2() => View();
        public IActionResult ShowEvents()
        {
            ViewBag.EventsTitle = "Live Events";
            return PartialView(_context.Events);
        }


        public IActionResult UseViewComponent1() => View();
        public IActionResult UseViewComponent2() => View();
        public IActionResult InjectServiceInView() => View();

    }
}
