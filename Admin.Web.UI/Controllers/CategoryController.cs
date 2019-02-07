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
   // [Authorize]//kategori sayfasına gitmeni engelliyor çünkü önce login olman lazım.yani admin girişi ile üye girişini ayırıyor.
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
       [HttpGet]
       [Authorize(Roles ="Admin")]//kategoriyi sadece admin eklemeli.
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                {//Eklediğimiz kategorinin kdv sini farklı girersek üstkategorinin kdv sini kategorinin kdvsine eşitliyor.
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id=0)
        {
            ViewBag.CategoryList = GetCategorySelectList();
            var data = new CategoryRepo().GetById(id);
            if(data==null)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Kategori bulunamadı",
                    ActionName = "Update",
                    ControllerName = "Category",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Update(Category model)
        {
            try
            {
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                if(!ModelState.IsValid)
                {
                    model.SupCategoryId = model.SupCategoryId ?? 0;
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }
                if(model.SupCategoryId>0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryId).TaxRate;
                }
                var data = new CategoryRepo().GetById(model.Id);
                data.CategoryName = model.CategoryName;
                data.TaxRate = model.TaxRate;
                data.SupCategoryId = model.SupCategoryId;
                new CategoryRepo().Update(data);
                foreach (var dataCategory in data.Categories)
                {
                    dataCategory.TaxRate = data.TaxRate;
                    new CategoryRepo().Update(dataCategory);
                    if (dataCategory.Categories.Any())
                        UpdateSubTaxRate(dataCategory.Categories);

                }
                void UpdateSubTaxRate(ICollection<Category> dataC)
                {
                    foreach (var dataCategory in dataC)
                    {
                        dataCategory.TaxRate = data.TaxRate;
                        new CategoryRepo().Update(dataCategory);
                        if (dataCategory.Categories.Any())
                            UpdateSubTaxRate(dataCategory.Categories);
                    }
                }
                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla güncellenmiştir.";
                ViewBag.CategoryList = GetCategorySelectList();
                return View(data);
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}