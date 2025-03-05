using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimTaiPhongThuy.Pages.Order
{
    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total => Subtotal - Discount;

        public void OnGet()
        {
            CartItems = new List<CartItem>
            {
                new CartItem { Name = "Vòng tay phong thủy", Code = "VT001", Quantity = 1, Price = 10000000, ImageUrl = "/images/vongtay.png" },
                new CartItem { Name = "Nhẫn bạc phong thủy", Code = "NB001", Quantity = 1, Price = 10000000, ImageUrl = "/images/nhanbac.png" }
            };
            Subtotal = CartItems.Sum(item => item.Price * item.Quantity);
            Discount = 0;
        }
    }

    public class CartItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}