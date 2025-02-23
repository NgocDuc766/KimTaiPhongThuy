using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using KimTaiPhongThuy.DataAccess;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Identity; // Thêm thư viện này

namespace KimTaiPhongThuy.Pages.Authentication
{
    public class AccountModel : PageModel
    {
        private readonly AuthenticationDAO _authDao;
        private readonly IPasswordHasher<User> _passwordHasher; // Thêm thuộc tính này
        private readonly EmailService _emailService; // Dịch vụ gửi email

        public AccountModel(AuthenticationDAO authDao, IPasswordHasher<User> passwordHasher, EmailService emailService)
        {
            _authDao = authDao;
            _passwordHasher = passwordHasher; // Khởi tạo password hasher
            _emailService = emailService;
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
            if (user == null)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            // So sánh mật khẩu đã mã hóa với mật khẩu người dùng nhập vào
            var result = _passwordHasher.VerifyHashedPassword(user, user.PassWord, Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            // Nếu mật khẩu chính xác, tạo cookies và chuyển hướng
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

            

            // Kiểm tra và thêm giá trị mặc định nếu các trường thiếu
            if (string.IsNullOrEmpty(User.Address))
            {
                User.Address = "Default Address"; // Hoặc lấy giá trị từ một trường nhập liệu trong form
            }

            if (string.IsNullOrEmpty(User.ContactName))
            {
                User.ContactName = "Default Contact Name"; // Giá trị mặc định
            }

            if (string.IsNullOrEmpty(User.PhoneNumber))
            {
                User.PhoneNumber = "000-000-0000"; // Giá trị mặc định cho số điện thoại
            }

            if (User.Gender == null) // Kiểm tra nếu chưa có giá trị cho Gender
            {
                User.Gender = false; // Hoặc true, tùy thuộc vào logic của bạn
            }

            if (User.DateOfBirth == null) // Kiểm tra nếu chưa có giá trị cho DateOfBirth
            {
                User.DateOfBirth = DateTime.Parse("2000-01-01"); // Hoặc ngày mặc định khác
            }

            // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
            User.PassWord = _passwordHasher.HashPassword(User, User.PassWord);

            User.RoleId = 2; // Cấp vai trò mặc định cho người dùng mới
            _authDao.RegisterUser(User); // Lưu thông tin người dùng vào cơ sở dữ liệu
            Message = "Registration successful. Please log in.";
            IsSignUp = false;
            return Page();
        }
    }
}