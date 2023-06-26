using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; //access to Json actions
using StoreFront.Data.EF.Models; //access to context and product classes
using StoreFront.UI.MVC.Models; //access to CartItemViewModel


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


            return View(shoppingCart);
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

        public IActionResult RemoveFromCart(int id) 
        {
            //Retreve cart session and convert to C#
            var sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,CartItemViewModel>>(sessionCart);

            //Remove the item
            shoppingCart.Remove(id);

            //If no items remaining in the cart, remove the cart from the session
            if (shoppingCart.Count ==0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //Otherwise update the session variable with the updated cart contents
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int id, int qty)
        {
            //Retrieve cart and convert to C#
            var sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Update the qty for the CartItemViewModel for the associated product key
            shoppingCart[id].Qty = qty;

            //Update the session with updated cart
            //If there are no remaining items in the cart, remove cart from the session
            if (shoppingCart[id].Qty == 0)
            {
                RemoveFromCart(id);
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SubmitOrder()
        {
            //Retrieve user ID
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            //Retrieve the UserDetails record
            User u = _context.Users.Find(userId);

            //Create the Order object and assign values
            Order o = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShipTo = u.FirstName + " " + u.LastName,
                City = u.City,
                State = u.State,
                Zip = u.Zip
            };

            //Add the order object to the _context (queues record to be saved in DB)
            _context.Orders.Add(o);

            //Retrieve session cart and convert to C#
            var sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Create OrderProduct object for each item in cart
            foreach (var item in shoppingCart)
            {
                OrderProduct op = new OrderProduct()
                {
                    ProductId = item.Key,
                    OrderId = o.OrderId,
                    Quantity = (short)item.Value.Qty,
                    ProductPrice = item.Value.Product.Price
                };

                //Add OP record to existing order entity since records are related
                o.OrderProducts.Add(op);

                _context.SaveChanges();
            }
            return RedirectToAction("Index","Orders");
        }

    }

}
