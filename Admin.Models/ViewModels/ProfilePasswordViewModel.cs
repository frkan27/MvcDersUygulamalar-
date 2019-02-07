using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.ViewModels
{
   public class ProfilePasswordViewModel
    {
        //iki viewmodeli birleştirmek için yaptık
        public UserProfileViewModel UserProfileVİewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}
