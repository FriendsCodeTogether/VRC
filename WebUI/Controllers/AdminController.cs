using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            string[] cars = { "car 1", "car 2", "car 3", "car 4", "car 5", "car 6", "car 7", "car 8" };
            return View(cars);
        }
    }
}
