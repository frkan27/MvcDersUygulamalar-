using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]//BaseController autorize olduğundan giren kişi hatayı görebilsin diye yaptık.
        public ActionResult Error()
        {

            return View();
        }
    }
}