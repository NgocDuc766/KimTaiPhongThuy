using KimTaiPhongThuy.Models;

namespace KimTaiPhongThuy.DataAccess
{
    public class AuthenticationDAO
    {
        private readonly JewelryStoreContext _context;

        public AuthenticationDAO(JewelryStoreContext context)
        {
            _context = context;
        }

        public bool ValidateUser(string username, string password)
        {
            return _context.Users.Any(u => u.UserName == username && u.PassWord == password);
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public void RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }
        public User? GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }
        // Phương thức lấy người dùng qua email
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
