﻿using Newtonsoft.Json;
using Rabbit.Model.Entities;
using Rabbit.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Publisher = Rabbit.Web.UI.Models.Publisher;

namespace Rabbit.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private static Publisher _publisher;
        public ActionResult Index()
        {
            var list = new List<Customer>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(new Customer()
                {
                    Name=Faker.NameFaker.Name(),
                    Surname=Faker.NameFaker.LastName(),
                    Address=Faker.TextFaker.Sentence(),
                    Email=Faker.InternetFaker.Email(),
                    Phone="5"+Faker.PhoneFaker.Phone().Replace("-","").Substring(1,9)
                });

            }
            var dataString = JsonConvert.SerializeObject(list);
            _publisher = new Publisher(dataString, "Customer");

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