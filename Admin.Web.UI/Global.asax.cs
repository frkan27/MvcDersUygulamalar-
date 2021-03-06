﻿using Admin.BLL.Identity;
using Admin.Models.Enums;
using Admin.Models.IdentityModels;
using Admin.Web.UI.App_Start;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Admin.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var roller = Enum.GetNames(typeof(IdentityRoles));
          //  var roller = new string[] { "Admin", "User" };//, koyup yeni rol ekleyebiliriz.
            var roleManager = MembershipTools.NewRoleManager();
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))//ctrl. ile import et RoleExist i.
                    roleManager.Create(new Role()//object ınıtialize la doldurma işlemi.
                    {
                        Name = rol
                    });
            }


        }
    }
}
