using BootstrapMvc.Controllers;
using PteMvc.Models;
using PteMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PteMvc.Controllers
{
    public class TruckController : BootstrapBaseController
    {
        private static List<TruckInputModel> _models = new TruckService().GetCurrent().ToList();

        public ActionResult Starter()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(_models);
        }

        [HttpPost]
        public ActionResult Create(TruckInputModel model)
        {
            return View("Create", model);
        }

        public ActionResult Delete(Guid id)
        {
            Information("Your truck was deleted");
            return RedirectToAction("index");
        }
        public ActionResult Edit(Guid id)
        {
            var model = _models.GetTruck(id);
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Edit(TruckInputModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
               return RedirectToAction("index");
            }
            Error("there were some errors in your form.");
            return View("Create", model);
        }

        public ActionResult Details(Guid id)
        {
            var model = _models.GetTruck(id);

            return View(model);
        }
    }
}
