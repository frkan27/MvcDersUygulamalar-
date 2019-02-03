using Admin.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.Entities
{
    [Table("Invoices")]
    public class Invoice:BaseEntity2<long,Guid>
    {
        [DisplayName("Birim")]
        public decimal Quantity { get; set; }
        [DisplayName("Fİyat")]
        public decimal Price { get; set; }
        [DisplayName("İndirim Oranı")]
        public decimal  Discount { get; set; }

        [ForeignKey("Id")]
        public virtual Order Order { get; set; }

        [ForeignKey("Id2")]
        public virtual Product Product{ get; set; }

        //ınvoice=fatura productla order arasında ara tablo görevini görücek.
        //BaseEntity2 oluşturcuk 1. ıd yi order a 2. ıd2 yi producta foreign key verdik.
        //bu yüzden diğer tarafta ınvoice lsitesi oluşturmamız lazım.
    }
}
