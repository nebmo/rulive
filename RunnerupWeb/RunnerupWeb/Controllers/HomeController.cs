using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvc.Controllers;

namespace RunnerupWeb.Controllers
{
    public class HomeController : BootstrapBaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Map", "Map"); 
        }
    }
}
