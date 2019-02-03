using Admin.BLL.Helpers;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
       [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Category model)
        {
            try
            {
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                if(!ModelState.IsValid)//normalde doğrulama yapılmış ise yapılacak işlemler burda yapılır
                {                     //ancak basında ! var bu  yüzden yapılmamış ise yapılcakalr burda.
                    ModelState.AddModelError("CategoryName", "100 karakteri aşamazsınız");
                    model.SupCategoryId = model.SupCategoryId ?? 0;//null değilse soldakine null sa sağdakine eşittir yani 0 dır.
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }
                if(model.SupCategoryId>0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryId).TaxRate;
                }
                new CategoryRepo().Insert(model);
                TempData["Message"] = $"{model.CategoryName} isimli kategori basarıyla eklenmiştir.";
                return RedirectToAction("Add");

            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text=$"Bir hata oluştu {EntityHelpers.ValidationMessage(ex)}",
                    ActionName="Add",
                    ControllerName="Category",
                    ErrorCode=500                   
                };
                return RedirectToAction("Error", "Home");
            }
            catch(Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}