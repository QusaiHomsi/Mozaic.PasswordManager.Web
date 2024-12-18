﻿using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Web.Models;
using System.Diagnostics;

namespace Mozaic.PasswordManager.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                
                return RedirectToAction("Hello");
            }

            
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Mainpage()
        {
            return View();
        }
        public ActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        public IActionResult Hello()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Unauthorized()
        {
            return View();
        }
        
    }
}
