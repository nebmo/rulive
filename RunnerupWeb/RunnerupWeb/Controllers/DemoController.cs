using System.Web.Mvc;
using BootstrapMvcSample.Controllers;

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
