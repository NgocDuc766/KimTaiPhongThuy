using KimTaiPhongThuy.Extension;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimTaiPhongThuy.Pages.Order
{
    public class IndexModel : PageModel
    {
        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }


        public void OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            TotalAmount = Cart.Sum(item => item.TotalPrice);
        }

        public IActionResult OnPostAddToCart(int productId, string productName, decimal price, string? imageUrl)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = 1,
                    ImageUrl = imageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            cart.RemoveAll(p => p.ProductID == productId);

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }
        public IActionResult OnPostIncreaseQuantity(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

        public IActionResult OnPostDecreaseQuantity(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

    }
}