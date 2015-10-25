using System.Web.Mvc;
using BootstrapMvcSample.Controllers;

namespace PteMvc.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Map", "Demo"); 
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View();
        }

        public ActionResult LogOff()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
