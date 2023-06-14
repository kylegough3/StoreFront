using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Data.EF.Models //.Metadata
{
    public class CategoryMetadata
    {
        //public int CategoryId { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [DisplayName("Category")]
        public string? Type { get; set; }
    }

    public class ManufacturerMetadata
    {
        //public int ManufacturerId { get; set; }

        [StringLength(100, ErrorMessage = "*Max 100 characters")]
        [DisplayName("Manufacturer")]
        public string? ManufacturerName { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [DisplayName("City")]
        public string? ManufacturerCity { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [DisplayName("State")]
        public string? ManufacturerState { get; set; }
    }

    public class OrderMetaData
    {
        //public int OrderId { get; set; }
        
        //public string? UserId { get; set; }

        [Required(ErrorMessage = "*Order Date is Required")]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "* Ship To is required")]
        [StringLength(100, ErrorMessage = "*Max 100 characters")]
        [Display(Name = "Ship To")]
        public string ShipTo { get; set; } = null!;

        [Required(ErrorMessage = "* City is required")]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        [Display(Name = "City")]
        public string City { get; set; } = null!;

        [StringLength(2, ErrorMessage = "*Max 2 characters")]
        [Display(Name = "State")]
        public string? State { get; set; }

        [Required(ErrorMessage = "* Zip is required")]
        [StringLength(5, ErrorMessage = "*Max 5 characters")]
        [Display(Name = "Zip")]
        public string? Zip { get; set; }
    }

    public partial class OrderProductsMetadata
    {
        //public int OrderProductId { get; set; }
        //public int ProductId { get; set; }
        //public int OrderId { get; set; }

        [Required(ErrorMessage = "*Quantity is Required")]
        [Range(0,(double)short.MaxValue)]
        public short? Quantity { get; set; }

        [Required(ErrorMessage = "*Price is Required")]
        [Display(Name ="Price")]
        [DataType(DataType.Currency)]
        [Range(0,(double)decimal.MaxValue)]
        public decimal? ProductPrice { get; set; }
    }

    public partial class ProductMetadata
    {
        //public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        //public int? CategoryId { get; set; }
        //public int? SizeId { get; set; }
        public decimal Price { get; set; }
        //public int? ManufacturerId { get; set; }
        //public int? StockStatusId { get; set; }

        [Display(Name="Qty In Stock")]
        public int? QtyInStock { get; set; }

        [StringLength(75)]
        [Display(Name="Image")]
        public string? ProductImage { get; set; }
    }

    public partial class StockStatusMetadata
    {
        //public int StockStatusId { get; set; }

        [StringLength(20, ErrorMessage = "*Max 20 characters")]
        public string? Status { get; set; }
    }

    public partial class UserMetadata
    {
        //public string UserId { get; set; } = null!;
        [Required(ErrorMessage ="*First Name is Required")]
        [Display(Name ="First Name")]
        [StringLength(50,ErrorMessage = "*Max 50 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "*Last Name is Required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "*Max 50 characters")]
        public string LastName { get; set; } = null!;

        
        [StringLength(150, ErrorMessage = "*Max 150 characters")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "*Max 50 characters")] //Sets a limit on the number of characters in the string
        [Display(Name = "City")]
        public string? City { get; set; }

        [StringLength(2, ErrorMessage = "*Max 2 characters")]
        [Display(Name = "State")]
        public string? State { get; set; }

        [StringLength(5, ErrorMessage = "*Max 5 characters")]
        [Display(Name = "Zip")]
        public string? Zip { get; set; }

        [StringLength(24, ErrorMessage = "*Max 24 characters")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
    }
}
