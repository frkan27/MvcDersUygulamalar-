using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.ViewModels
{
  public  class RegisterLoginViewModel
    {
        //birden fazla model göndermek istersek anasayfa vey baska bir yere multipleviewmodel yaparız.
        //bu classı cağırırsak ikisi birden kullanılmış olur.
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }
}
