using Microsoft.AspNetCore.Identity;
using System;
using TTBackEnd.Shared;

namespace TTFrontEnd.Models.DTO
{
    public partial class User : DataContext.Users
    {
        public string Password { get; set; }

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

        public Tuple<bool, string> ChangePassword(string oldPassword, string newPassword)
        {
            LoginModel login = new LoginModel() { Email = this.Email, Password = oldPassword };
            var rs = VerifyPassword(login);
            if (rs == PasswordVerificationResult.Success)
            {
                return new Tuple<bool, string>(true, SetPassword(newPassword));
            }
            return new Tuple<bool, string>(false, string.Empty);
        }
    }
}