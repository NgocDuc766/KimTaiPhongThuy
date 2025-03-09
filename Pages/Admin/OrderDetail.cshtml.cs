using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KimTaiPhongThuy.Models;
using System.Linq;
using System.Threading.Tasks;

namespace KimTaiPhongThuy.Pages.Admin
{
    public class OrderDetailModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public OrderDetailModel(JewelryStoreContext context)
        {
            _context = context;
        }

        public Models.Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = 2; 
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Order");
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = 0; 
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Order");
        }
    }
}