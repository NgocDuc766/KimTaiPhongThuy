using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KimTaiPhongThuy.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public IndexModel(JewelryStoreContext context)
        {
            _context = context;
        }

        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }

        public void OnGet()
        {
            TotalOrders = _context.Orders.Count();
            TotalProducts = _context.Products.Count();
            TotalUsers = _context.Users.Count();
        }
    }
}
