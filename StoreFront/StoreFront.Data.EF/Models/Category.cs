using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string? Type { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
