using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    [Authorize]//Basecontrolerden kalıtım almış tüm ekranlarım mutlaka giriş yapılmış olan ekranlar olmalı.
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetCategorySelectList()
        {
            var categories = new CategoryRepo()//supcategoryıd si null olanları getir.
                .GetAll(x => x.SupCategoryId == null)
                .OrderBy(x => x.CategoryName);//Üst kategorisi bulunmayanlar geliyor.
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="Üst Kategorisi yok",
                    Value="0"
                }
            };
            foreach (var category in categories)//üstkategorisi null olanlar içinde dönüyorum.
            {
                if(category.Categories.Any())//eğer altcategorisi olan varsa.(Meyvenin içinde turucgil var)
                {
                    list.Add(new SelectListItem()//textine bulduğum categoryname i valusunede ıd yi atıyorum.
                    {
                        Text = category.CategoryName,
                        Value = category.Id.ToString()
                    });//Meyvenin içinde turuncgil olduğunda getsubcategories çalışıyor.
                    list.AddRange(GetSubCategories(category.Categories.OrderBy(x => x.CategoryName).ToList()));
                                                 //category unlu mamuller oldu. unlumamullerin içinde ekmek var.category.categories bunu temsil eder.
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = category.CategoryName,
                        Value = category.Id.ToString()
                    });
                }
            }
            List<SelectListItem> GetSubCategories(List<Category> categories2)
            {
                var list2 = new List<SelectListItem>();
                foreach (var category in categories2)//Meyve kategorisinin içinde turunçgil kategorisi var.
                {
                    if (category.Categories.Any())//turuncgilin altında kategori olmadığında bu if i geçiyor.
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = category.CategoryName,
                            Value = category.Id.ToString()
                        });
                        list2.AddRange(GetSubCategories(category.Categories.OrderBy(x => x.CategoryName).ToList())); 
                    }
                    else
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = category.CategoryName,
                            Value = category.Id.ToString()
                        });
                    }
                }
                return list2;
            }
            return list;
        }

        protected List<SelectListItem> GetProductSelectList()
        {
            var products = new ProductRepo()
                .GetAll(x => x.SupProductId == null && x.ProductType == ProductTypes.Retail)
                .OrderBy(x => x.ProductName);
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Perakende Ürünü Yok",
                    Value = new Guid().ToString()
                }
            };
            foreach (var product in products)
            {
                if (product.Products.Any(x => x.ProductType == ProductTypes.Retail))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = product.ProductName,
                        Value = product.Id.ToString()
                    });
                    list.AddRange(GetSubProducts(product.Products.Where(x => x.ProductType == ProductTypes.Retail).OrderBy(x => x.ProductName).ToList()));
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = product.ProductName,
                        Value = product.Id.ToString()
                    });
                }
            }

            List<SelectListItem> GetSubProducts(List<Product> products2)
            {
                var list2 = new List<SelectListItem>();
                foreach (var product in products2)
                {
                    if (product.Products.Any(x => x.ProductType == ProductTypes.Retail))
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = product.ProductName,
                            Value = product.Id.ToString()
                        });
                        list2.AddRange(GetSubProducts(product.Products.Where(x => x.ProductType == ProductTypes.Retail).OrderBy(x => x.ProductName).ToList()));
                    }
                    else
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = product.ProductName,
                            Value = product.Id.ToString()
                        });
                    }
                }
                return list2;
            }

            return list;
        }
    }
}