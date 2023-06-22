using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Size
    {
        public Size()
        {
            Products = new HashSet<Product>();
        }

        public int SizeId { get; set; }
        public string? SizeName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
