using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KimTaiPhongThuy.Models;
using System.Collections.Generic;
using System.Linq;

namespace KimTaiPhongThuy.Pages.Admin
{
    public class OrderModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public OrderModel(JewelryStoreContext context)
        {
            _context = context;
        }

        public IList<Models.Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
        }
    }
}