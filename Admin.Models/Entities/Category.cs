using Admin.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.Entities
{
    [Table("Categories")]
   public class Category:BaseEntity<int>
    {
        [StringLength(100,ErrorMessage ="Kategori Adı 3 ile 100 karakter arasında olmalı",MinimumLength =3)]
        [DisplayName("Kategori Adı")]
        [Required]
        public string CategoryName { get; set; }
        [Range(0,99)]
        [DisplayName("Kdv Oranı")]
        public decimal TaxRate  { get; set; }

        [DisplayName("Üst Kategori")]
        public int SupCategoryId { get; set; }
        [ForeignKey("SupCategoryId")]
        public virtual  Category SupCategory { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();






    }
}
