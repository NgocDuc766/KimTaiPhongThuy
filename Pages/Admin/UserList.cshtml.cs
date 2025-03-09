using KimTaiPhongThuy.DataAccess;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace KimTaiPhongThuy.Pages.Admin
{
    public class UserListModel : PageModel
    {
        private readonly AuthenticationDAO _authenticationDAO;

        public UserListModel(AuthenticationDAO authenticationDAO)
        {
            _authenticationDAO = authenticationDAO;
        }

        public List<User> Users { get; set; } = new List<User>();

        public int TotalUserCount { get; set; }

        public void OnGet()
        {
            Users = _authenticationDAO.GetAllUsers().ToList();

            TotalUserCount = Users.Count();
        }
    }
}
