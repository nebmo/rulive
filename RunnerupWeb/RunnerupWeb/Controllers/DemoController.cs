using BootstrapMvc.Controllers;
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
        private static List<DemopInputModel> _models = GetDemoList();
        
        public ActionResult Index()
        {
            return View(_models);
        }

        //[HttpPost]
        //public ActionResult Create(TruckInputModel model)
        //{
        //    return View();
        //}

        public ActionResult Delete(Guid id)
        {
            Information("Your truck was deleted");
            return RedirectToAction("index");
        }
        public ActionResult Edit(Guid id)
        {
            return View("Create", _models.First(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult Edit(DemopInputModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
               return RedirectToAction("index");
            }
            return View("Create", model);
        }

        public ActionResult Details(Guid id)
        {
            return View(_models.First(x => x.Id == id));
        }

        //public ActionResult Starter()
        //{
        //    return View();
        //}

        public ActionResult Map()
        {
            return View();
        }

        
        private static List<DemopInputModel> GetDemoList()
        {
            var result = new List<DemopInputModel>();

            result.Add(new DemopInputModel()
            {
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ForName = "Niklas",
                LastName = "Weidemann",
                EmailAddress = "niklas.weidemann@pocketmobile.se",
                Password = "******",
                Age = 54,
                ZipCode = 43651,
                UserName = "BootstrapGuru"
            });

            result.Add(new DemopInputModel()
            {
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ForName = "Tommy",
                LastName = "Linsemark",
                EmailAddress = "tommy.linsemark@pocketmobile.se",
                Password = "******",
                Age = 27,
                ZipCode = 50630,
                UserName = "DevCeleb"
            });

            return result;
        }
    }

    public class DemopInputModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [LocalizedDisplayName("labelForName")]
        public string ForName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Range(0, 130)]
        public int Age { get; set; }

        [RegularExpression("^[0-9]{5}", ErrorMessage = "Please enter your five digit postal code")]
        public int ZipCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceId): base(GetMessageFromResource(resourceId))
        { }

        private static string GetMessageFromResource(string resourceId)
        {
            return "Förnamn";
        }
    }
}
