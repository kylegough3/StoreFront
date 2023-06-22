using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreFront.Data.EF.Models; //access to context and product classes
using StoreFront.UI.MVC.Models; //access to CartItemViewModel
using Newtonsoft.Json; //access to Json actions

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly Gough_StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(Gough_StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Retrieve the session cart contents
            var sessionCart = HttpContext.Session.GetString("cart");

            //Create the empty shell for the local (C#) cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //Check to see if the cart is null
            if (sessionCart == null || sessionCart.Count() == 0) 
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "Cart is currently empty";
            }
            else
            {
                ViewBag.Message = null;

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,  CartItemViewModel>>(sessionCart);
            }


            return View(sessionCart);
        }

        public IActionResult AddToCart(int id)
        {
            //int (Dictionary Key) -> product id
            //CartItemViewModel (Dictionary value) -> Qty and Product
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            var sessionCart = HttpContext.Session.GetString("cart");

            //Checks to see if the session already has a Cart with values. If not, instantiates one
            if (String.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                //Transfer existing cart into the local shopping cart dictionary
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            //Add the selected product to the cart
            //Retrieve product from the database
            Product product = _context.Products.Find(id);

            CartItemViewModel civm = new CartItemViewModel(1, product); //Adds 1 of product to cart

            //If product is already in cart, increase qty
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }
            //Update session with new version of cart taking the local cart and serializing as JSON
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }



    }

}
