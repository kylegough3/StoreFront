using StoreFront.Data.EF.Models; //Grants access to products and data

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }

        public Product Product { get; set; }

        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            Product = product;
        }
    }
}
