using Microsoft.AspNetCore.Identity;
using Tastee.Shared;

namespace Tastee.Models.DTO
{
    public partial class User : Tastee.Infrastucture.Data.Context.Users
    {
        public PasswordVerificationResult VerifyPassword(LoginModel login)
        {
            var passwordHasher = new PasswordHasher<LoginModel>();
            var verifyResult = passwordHasher.VerifyHashedPassword(login, PasswordHash, login.Password);
            return verifyResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="password"></param>
        /// <returns>HashPassword</returns>
        public string SetPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(this, password);
        }
    }
}