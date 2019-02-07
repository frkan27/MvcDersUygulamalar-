using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Ad")]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        [StringLength(25)]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Telefon No")]
        public string PhoneNumber { get; set; }

    }
}
