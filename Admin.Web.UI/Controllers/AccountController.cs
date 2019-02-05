using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Admin.BLL.Identity.MembershipTools; // static nesnemizi aşağıda kolayca çağırmak için bu sekilde kullanabiliriz.
using Admin.Models.IdentityModels;
//staticler tektir ve kolay çağrılır.
namespace Admin.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Register(RegisterLoginViewModel model)

        {
            if(!ModelState.IsValid)
            {
                return View("Index", model);//ındex view ını modelle çalıştıryprum.
            }

            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();//yukarda static kütüphane seklinde tanımlamasaydık MembershipTools.Newusermanager yazardık.
                var roleManager = NewRoleManager();
                //registerviewmodelde propertlere tanımladığımız isim username ... leri formda doldurup rm ye atıyoruz.
                var rm = model.RegisterViewModel;//Kullanıcı varmı yokmu diye bakmak için kullanıyoruz.
                var user = await userManager.FindByNameAsync(rm.UserName);//username e göre arıyor.

                if (user != null)
                {
                    ModelState.AddModelError("UserName", "Bu kullanıcı daha önce alınmıştır.");
                    return View("Index", model);

                }
                var newUser = new User()//Kullanıcıyı değişkene atıyprum.
                {
                    UserName = rm.UserName,
                    Email = rm.Email,
                    Name = rm.Name,
                    Surname = rm.Surname
                };
                //Kullanıcı oluşturuyoruz.
                var result = await userManager.CreateAsync(newUser, rm.Password);//password ü ayrı alıyor çünkü şifrelicek.

                if(result.Succeeded)//İlk kullanıcıysa admin yapalım.
                {
                    if(userStore.Users.Count()==1)
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "Admin");//içine userıd ve role alır.
                    }
                    else//ilk kaydettiğimiz admin oldu sonrakiler user olarak buraya düşüyor.
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "User");
                    }
                    //todo kullanıcıya mail göndersin.
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    return View("Index", model);
                }
                TempData["Message"] = "Kaydınız basarıyla alınmıştır"; 
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");

            }




        }

    }
}