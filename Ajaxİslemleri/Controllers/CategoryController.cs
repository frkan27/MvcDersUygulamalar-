using Ajaxİslemleri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ajaxİslemleri.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public JsonResult Search(string s)
        {
            var key = s.ToLower();
            if(key.Length<=2)
            {
                return Json(new ResponseData()
                {
                    message = "Aramak için 2 karakaterden fazla girmelisiniz.",
                    success = false
                }, JsonRequestBehavior.AllowGet);                        
            }

            try
            {
                var db = new NorthwindEntities();
                db.Configuration.LazyLoadingEnabled = false;
                var data = db.Categories                      //contains() içine girilen string ifadeyi arar.
                    .Where(x => x.CategoryName.ToLower().Contains(key) || x.Description.ToLower().Contains(key))
                    .ToList();
                return Json(new ResponseData()
                {
                    message = $"{data.Count} adet kayıt bulundu",
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu {ex.Message}",
                    success = false,
                   
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}