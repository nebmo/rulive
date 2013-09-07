﻿using BootstrapMvc.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace RunnerupWeb.Controllers
{
    public class DemoController : BootstrapBaseController
    {
        public ActionResult Map()
        {
            return RedirectToAction("Map", "Map"); 
        }

    }
}
