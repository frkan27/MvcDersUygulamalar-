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
using Admin.BLL.Services.Senders;
using Admin.BLL.Helpers;
using System.Web.ModelBinding;
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
                var userStore = NewUserStore();//veritabanı işlemi yapcağımız için Store kullanırız.
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
                    Surname = rm.Surname,
                    ActivationCode=StringHelpers.GetCode()
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
                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var emailService = new EMailService();
                    var body = $"Merhaba <b>{newUser.Name} {newUser.Surname}</b><br>Hesabınızı aktif etmek için aşadıdaki linke tıklayınız<br> <a href='{SiteUrl}/account/activation?code={newUser.ActivationCode}' >Aktivasyon Linki </a> ";
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Sitemize Hoşgeldiniz" }, newUser.Email);//mailin gitmesi için newUser.Email eklemeliyiz.
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
        //Kayıt olan kullanıcyla giriş işlemi.
        public async Task<ActionResult> Login(RegisterLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }
                var userManager = NewUserManager();//Role işleri için usermanager kullanılır.
                var lm = model.LoginViewModel;
                var user = await userManager.FindAsync(lm.UserName, lm.Password);//Login için kullanıcı adı ve şifreye bakıyor.
                //Kullanıcı adı ve şifre alınmamıssa buraya giriyor.
                if(user==null)
                {
                    ModelState.AddModelError("","Kullanıcı Adı veya şifre hatalı");
                    return View("Index", model);
                }
                //Kimlik doğrulama için Auhhetication Manager a ihtiyacım var.İçinde signin signout var .
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity =//Kopyala yapıştır.ApplicationCookie uygulama içi çerezler.
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
                //kullanıcının ıd sinden kullanıcnın bilgilerini çekicez.
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();//Bize bağlı olan kullanıcnın ıd sini vericek.
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
                //Değiştirme işlemleri burda yapılıyor.
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
                {//kullanıcı bilgilerini günncelleyen sayfayla aynı sayfa olduğundan kullanıcı bilgilerini doldururuyoruz.
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

        [HttpGet]
        [AllowAnonymous]
        //Maile aktivasyon kodu gelsin diye yaptık.
        public ActionResult Activation(string code)
        {
            try
            {
                var userStore = NewUserStore();
                var user = userStore.Users.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        ViewBag.Message = $"<span class='alert alert-success'>Bu hesap daha önce aktive edilmiştir.</span>";
                    }
                    else
                    {
                        user.EmailConfirmed = true;

                        userStore.Context.SaveChanges();
                        ViewBag.Message = $"<span class='alert alert-success'>Aktivasyon işleminiz başarılı</span>";
                    }
                }
                else
                {
                    ViewBag.Message = $"<span class='alert alert-danger'>Aktivasyon başarısız</span>";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "<span class='alert alert-danger'>Aktivasyon işleminde bir hata oluştu</span>";
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        //Şifremi unuttum .
        public ActionResult RecoverPassword()
        {
            return View();  // get ini oluşturup view unu ekliyoruz. Sayfa düzenini ayarlaıyoruz ve sonrada postunu yazıyoruz.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> RecoverPassword(RecoverPasswordViewModel model)//RecoverPasswordViewModel içindekileri model nesnemle ulaşabilirim.
        {
            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();
                var user = await userStore.FindByEmailAsync(model.Email);

                if(user==null)
                {
                    ModelState.AddModelError(string.Empty, $"{model.Email} mail adresine kayıtlı bir üyeliğe erişilemedi");
                    return View(model);
                }
                var newPassword = StringHelpers.GetCode().Substring(0, 6);//Yeni şifre veriyoruz ilk 6 hanesini
                await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(newPassword));
                var result= userStore.Context.SaveChanges();
                if (result == 0)
                {
                    TempData["Model"] = new ErrorViewModel()
                    {
                        Text = $"Bir hata oluştu",
                        ActionName = "RecoverPassword",
                        ControllerName = "Account",
                        ErrorCode = 500
                    };
                    return RedirectToAction("Error", "Home");
                }

                var emailService = new EMailService();
                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Hesabınızın parolası sıfırlanmıştır<br> Yeni parolanız: <b>{newPassword}</b> <p>Yukarıdaki parolayı kullanarak sistemize giriş yapabilirsiniz.</p>";
                emailService.Send(new IdentityMessage() { Body = body, Subject = $"{user.UserName} Şifre Kurtarma" }, user.Email);


            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "RecoverPassword",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }



            return View();
        }
    }
}