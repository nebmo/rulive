using System.Web.Mvc;
using BootstrapMvcSample.Controllers;

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
