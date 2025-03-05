using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimTaiPhongThuy.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa session và cookie khi logout
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserRole");

            // Chuyển hướng về trang chủ sau khi logout
            return RedirectToPage("/Index");
        }
    }
}
