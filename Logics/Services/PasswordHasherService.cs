using DomainModels.Models;
using Logics.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public User_ HashPassword(string password, User_ user)
        {
            var hasher = new PasswordHasher<User_>();
            user.HashPassword = hasher.HashPassword(user, password);
            return user;
        }

        public bool CheckHashPassword(User_ user, string password)
        {
            var hasher = new PasswordHasher<User_>();
            var result = hasher.VerifyHashedPassword(user, user.HashPassword, password);
            if (result != PasswordVerificationResult.Success)
            {
                return false;
            }
            return true;
        }
    }
}
