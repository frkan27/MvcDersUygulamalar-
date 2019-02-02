using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCilkProje.Models
{
    public class Kisi
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}