using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using KimTaiPhongThuy.DataAccess;
using KimTaiPhongThuy.Models;

namespace KimTaiPhongThuy.Pages.Authentication
{
    public class AccountModel : PageModel
    {
        private readonly AuthenticationDAO _authDao;

        public AccountModel(AuthenticationDAO authDao)
        {
            _authDao = authDao;
        }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public User User { get; set; } = new User();

        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public bool IsSignUp { get; set; } = false;

        public void OnGet(string? mode)
        {
            IsSignUp = mode == "register";
        }

        public IActionResult OnPostLogin()
        {
            var user = _authDao.GetUserByUsername(UserName);
            if (user == null || user.PassWord != Password)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,  // Đảm bảo hoạt động trên HTTPS
                SameSite = SameSiteMode.Strict // Chặn cookie gửi từ trang khác
            };

            // Lưu cookie trong session (mất khi tắt trình duyệt)
            Response.Cookies.Append("UserId", user.UserId.ToString(), cookieOptions);
            Response.Cookies.Append("UserRole", user.RoleId.ToString(), cookieOptions);

            return user.RoleId == 1 ? RedirectToPage("/Admin/Dashboard") : RedirectToPage("/Index");
        }




        public IActionResult OnPostRegister()
        {
            if (_authDao.UserExists(User.UserName))
            {
                Message = "Username already exists.";
                IsSignUp = true;
                return Page();
            }

            User.RoleId = 2;
            _authDao.RegisterUser(User);
            Message = "Registration successful. Please log in.";
            IsSignUp = false;
            return Page();
        }
    }
}
