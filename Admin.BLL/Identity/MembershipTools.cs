using Admin.DAL;
using Admin.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.BLL.Identity
{
    public static class MembershipTools
    {
        private static MyContext _db;
        //UserStore lazım olduğu zaman NewUserStore() metodunu kullanacam.
        //(_db ?? new MyContext()); daha önceden ınstance alınmıssa (_db) kullanıyorum. alınmamıssa yeniden alınmasını salıyorum.
        public static UserStore<User> NewUserStore() => new UserStore<User>(_db ?? new MyContext());
        public static UserManager<User> NewUserManager() => new UserManager<User>(NewUserStore());

        public static RoleStore<Role> NewRoleStore() => new RoleStore<Role>(_db ?? new MyContext());
        public static RoleManager<Role> NewRoleManager() => new RoleManager<Role>(NewRoleStore());
    }
}
