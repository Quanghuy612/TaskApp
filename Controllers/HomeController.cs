﻿using Microsoft.AspNetCore.Mvc;

namespace TaskApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
