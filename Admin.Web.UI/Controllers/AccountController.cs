using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Admin.BLL.Identity.MembershipTools; // static nesnemizi aşağıda kolayca çağırmak için bu sekilde kullanabiliriz.
using Admin.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//staticler tektir ve kolay çağrılır.
namespace Admin.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            //sisteme girdikten sonra tekrar account yazarsak yine home ındex e yönlendiriyo çünkü çıkış yapmadık.
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)//kullanıcı sisteme giriş yapmış oluoyr.true ise.
                return RedirectToAction("Index", "Home");//Kullanıcı varsa bizi home indexe yönlendirsin.
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

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Login(RegisterLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }
                var userManager = NewUserManager();
                var lm = model.LoginViewModel;
                var user = await userManager.FindAsync(lm.UserName, lm.Password);//Login için kullanıcı adı ve şifreye bakıyor.
                //Kullanıcı adı ve şifre alınmamıssa buraya giriyor.
                if(user==null)
                {
                    ModelState.AddModelError("","Kullanıcı Adı veya şifre hatalı");
                    return View("Index", model);
                }
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity =
                    await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = model.LoginViewModel.RememberMe
                }, userIdentity);
                return RedirectToAction("Index", "Home");
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
        [HttpGet]
        public ActionResult Logout()// /Account/logout yazınca tekrar login -register giriş sayfasına yönlendiriyor.
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index");
        }



        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();//Bİze bağlı olan kullanıcnın ıd sini vericek.
                var user = NewUserManager().FindById(id);
                var data = new ProfilePasswordViewModel()
                {
                    UserProfileVİewModel=new UserProfileViewModel()
                    {
                        Email=user.Email,
                        Id=user.Id,
                        Name=user.Name,
                        Surname=user.Surname,
                        PhoneNumber=user.PhoneNumber,
                        UserName=user.UserName
                    }
                };
                return View(data);

            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<ActionResult> UpdateProfile(ProfilePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }
            try
            {//Kullanıcıyı bulmak için usermanager e ihtiyacım var.

                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(model.UserProfileVİewModel.Id);

                user.Name = model.UserProfileVİewModel.Name;
                user.Surname = model.UserProfileVİewModel.Surname;
                user.PhoneNumber = model.UserProfileVİewModel.PhoneNumber;
                if(user.Email!=model.UserProfileVİewModel.Email)
                {
                    //todo tekrar aktivasyon kodu gönderilmeli ve tekrar aktif olmamış üye rolüne dönmeli.
                }
                user.Email = model.UserProfileVİewModel.Email;

                await userManager.UpdateAsync(user);
                TempData["Message"] = "Güncelle işlemi başarılı";
                return RedirectToAction("UserProfile");


            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<ActionResult> ChangePassword(ProfilePasswordViewModel model)
        {
            try
            {
                var userManager = NewUserManager();//kullanıcı repom. userla ilgili işlemler burdan gelişyo.
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var data = new ProfilePasswordViewModel()
                {
                    UserProfileVİewModel = new UserProfileViewModel()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Surname = user.Surname,
                        UserName = user.UserName
                    }
                };
                model.UserProfileVİewModel = data.UserProfileVİewModel;
                if (!ModelState.IsValid)
                {
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }


                var result = await userManager.ChangePasswordAsync(
                    HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId(),
                    model.ChangePasswordViewModel.OldPassword, model.ChangePasswordViewModel.NewPassword);

                if (result.Succeeded)
                {
                    //todo kullanıcıyı bilgilendiren bir mail atılır
                    return RedirectToAction("Logout", "Account");//şifreyi güncelleyince login sayfasına atıyor.
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

    }
}