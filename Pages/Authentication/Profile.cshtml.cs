using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KimTaiPhongThuy.Pages.Authentication
{
    public class ProfileModel : PageModel
    {
        private readonly JewelryStoreContext _context;

        public ProfileModel(JewelryStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToPage("/Authentication/Login"); // Redirect to login if not authenticated
            }

            User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToPage("/Authentication/Login");
            }

            if (ModelState.IsValid)
            {
                var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

                if (userToUpdate != null)
                {
                    userToUpdate.UserId = userToUpdate.UserId;
                    userToUpdate.UserName = userName;
                    userToUpdate.ContactName = User.ContactName;
                    userToUpdate.Email = User.Email;
                    userToUpdate.Address = User.Address;
                    userToUpdate.Gender = User.Gender;
                    userToUpdate.DateOfBirth = User.DateOfBirth;
                    userToUpdate.PhoneNumber = User.PhoneNumber;

                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Profile"); // Redirect after successful update
                }
            }
            return Page(); // If validation fails, stay on the page
        }
    }

}
