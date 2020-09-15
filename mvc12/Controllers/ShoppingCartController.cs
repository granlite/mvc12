using muscshop.Context;
using muscshop.Helper;
using muscshop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace muscshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private StoreContext _storeContext = new StoreContext();
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cartViewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(cartViewModel);
        }
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var album = _storeContext.Albums.Where(x => x.AlbumId == id).FirstOrDefault();
            var currcart = ShoppingCart.GetCart(this.HttpContext);
            currcart.AddToCart(album);
            _storeContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}