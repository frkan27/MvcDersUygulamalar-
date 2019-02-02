using MVCilkProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCilkProje.Controllers
{
    public class HomeController : Controller
    {
        static List<Kisi> kisiler = new List<Kisi>()
        {
            new Kisi
            {
                Name="Furkan",
                Surname="Kılıç"
            },
             new Kisi
            {
                Name="Kürsad",
                Surname="bayır"
            },
              new Kisi
            {
                Name="Fatih",
                Surname="Obut"
            },
        };
        [HttpGet]
        public ActionResult Liste()
        {
            return View(kisiler);
        }
        public ActionResult Detail(Guid? id)
        {
            var kisi = kisiler.FirstOrDefault(x => x.Id == id);
            if (kisi == null)
                return RedirectToAction("Liste");
            return View(kisi);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}