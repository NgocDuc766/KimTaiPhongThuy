using KimTaiPhongThuy.DataAccess.DTO;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KimTaiPhongThuy.Pages.Authentication
{
    [Route("/Authentication/Profile")]
    public class ProfileModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public ProfileModel(JewelryStoreContext context)
        {
            _context = context;
        }

        // Property để bind dữ liệu UserDto từ Razor Page
        [BindProperty]
        public UserDto User { get; set; }

        // OnGet: Lấy thông tin người dùng từ cơ sở dữ liệu và gán vào UserDto
        public IActionResult OnGet()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToPage("/Authentication/Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                User = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    ContactName = user.ContactName,
                    Email = user.Email,
                    Address = user.Address,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber
                };
            }
            else
            {
                return RedirectToPage("/Authentication/Login");
            }

            return Page();
        }

        // OnPost: Xử lý khi người dùng bấm nút "Update Profile"
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserId == User.UserId);
                    if (user != null)
                    {
                        // Cập nhật thông tin người dùng
                        user.ContactName = User.ContactName;
                        user.Email = User.Email;
                        user.Address = User.Address;
                        user.Gender = User.Gender;
                        user.DateOfBirth = User.DateOfBirth;
                        user.PhoneNumber = User.PhoneNumber;
                        user.UpdatedAt = DateTime.Now;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        _context.SaveChanges();

                        // Chuyển hướng đến trang profile sau khi cập nhật thành công
                        return RedirectToPage("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found.");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu có vấn đề xảy ra
                    ModelState.AddModelError("", "An error occurred while updating the profile: " + ex.Message);
                    return Page();
                }
            }

            // Nếu ModelState không hợp lệ, giữ lại trang hiện tại và hiển thị lỗi
            return Page();
        }
    }
}
