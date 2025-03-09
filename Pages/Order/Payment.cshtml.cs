using KimTaiPhongThuy.Extension;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KimTaiPhongThuy.Pages.Order
{
    public class PaymentModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public PaymentModel(JewelryStoreContext context)
        {
            _context = context;
        }

        public List<CartItem> Cart { get; set; }
        public int UsersId { get; set; }
        public int? UsesId { get; set; }


        public IActionResult OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            UsersId = int.Parse(Request.Cookies["UserId"]);
            UsesId = HttpContext.Session.GetInt32("UsersId");
            return Page();
        }

        public IActionResult OnPost()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (Cart == null || !Cart.Any())
                return RedirectToPage("Cart");
            UsersId = int.Parse(Request.Cookies["UserId"]);
            if (UsersId == 0)
                return RedirectToPage("/Products/ProductList");
            var order = new Models.Order
            {
                UserId = UsersId,
                OrderDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                Status = 1,
                CreatedBy = UsersId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in Cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");

            return RedirectToPage("CheckOut");
        }
    }
}
