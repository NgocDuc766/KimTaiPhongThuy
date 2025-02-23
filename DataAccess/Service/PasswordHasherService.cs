using Microsoft.AspNetCore.Identity;

namespace KimTaiPhongThuy.DataAccess.Service
{
    public class PasswordHasherService
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordHasherService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(string plainPassword)
        {
            return _passwordHasher.HashPassword(null, plainPassword);
        }

        public bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
