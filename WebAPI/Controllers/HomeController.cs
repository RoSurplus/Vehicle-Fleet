//-----------------------------------------------------------------------
// <copyright file="HomeController.cs">
//  Copyright (c) 2017 All Rights Reserved
//  <author>Kenneth Larimer</author>
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
