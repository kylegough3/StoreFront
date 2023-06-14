using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }

        public int ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public string? ManufacturerCity { get; set; }
        public string? ManufacturerState { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
