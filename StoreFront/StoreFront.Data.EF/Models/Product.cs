using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? SizeId { get; set; }
        public decimal Price { get; set; }
        public int? ManufacturerId { get; set; }
        public int? StockStatusId { get; set; }
        public int? QtyInStock { get; set; }
        public string? ProductImage { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual Size? Size { get; set; }
        public virtual StockStatus? StockStatus { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
