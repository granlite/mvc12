using muscshop.Context;
using muscshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace muscshop.Helper
{
    public class ShoppingCart
    {
        private StoreContext _storeContext = new StoreContext();
        private string ShoppingCartId { get; set; }
        private const string CartSessionKey = "CartId";
        public static string GetCartId (HttpContextBase httpContext)
        {
            if (httpContext.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
                {
                    httpContext.Session[CartSessionKey] = httpContext.User.Identity.Name;
                }
                else
                {
                    var temporaryCartId = Guid.NewGuid();
                    httpContext.Session[CartSessionKey] = Guid.NewGuid().ToString();
                }
            }
            return httpContext.Session[CartSessionKey].ToString();
        }
        public decimal GetTotal()
        {
            var total = (from cartItems in _storeContext.Carts 
                         where cartItems.CartId == this.ShoppingCartId 
                         select (int?) cartItems.Count * cartItems.Album.Price).Sum();
            return 12;
        }
        public List<Cart> GetCartItems()
        {
            var result = _storeContext.Carts.Include("Album").Where(x => x.CartId == this.ShoppingCartId).ToList();
            return result;
        }
        public static ShoppingCart GetCart(HttpContextBase httpContext)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = GetCartId(httpContext);
            return cart;
        }
        public void AddToCart(Album album)
        {
            var cartItem = _storeContext.Carts.SingleOrDefault(x=>x.CartId == ShoppingCartId && x.AlbumId == album.AlbumId);
            if(cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    CreateDate = DateTime.Now
                };
            }
            else
            {
                cartItem.Count++;
            }
            _storeContext.SaveChanges();
        }
    }
}