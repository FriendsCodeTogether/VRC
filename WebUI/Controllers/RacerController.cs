using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RacerController : Controller
    {
        private int countdownCounter = 3;
        public IActionResult Index()
        {
            ViewData.Add("counter", countdownCounter);
            return View();
        }

        public int Countdown()
        {
            countdownCounter--;
            return countdownCounter;
        }
    }
}
