using DataAccess.Repositories;
using DomainModels.Models;
using Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Logics.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userrep;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasherService _passwordHasher;
        public UserService(UserRepository userrep,
            IHttpContextAccessor httpContextAccessor,
            IPasswordHasherService passwordHasher) 
        {
            _userrep = userrep;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;

        }
        public async Task<List<User_>> GetAllUsers()
        {
            return await _userrep.GetAll();
        }
        public async Task<User_?> GetUserById(Guid Id)
        {
            var users = await _userrep.GetAll();
            var user = users.FirstOrDefault(u => u.Id==Id);

            return user;
        }
        public async Task<Guid> UserReg(User_ newuser)
        {
            var users = await _userrep.GetAll();
            var chekNameUser = users.FirstOrDefault(u => u.Name == newuser.Name);
            if (chekNameUser != null)
            {
                return Guid.Empty;
            }
            var validUser = _passwordHasher.HashPassword(newuser.HashPassword, newuser);
            return await _userrep.Create(validUser);
        }
        public async Task<Guid> UserUpdate(Guid id, string newName,string newAbout)
        {
            await _userrep.Update(id, newName, newAbout);
            return id;
        }


        public async Task<Guid> UserLogin(string name, string password)
        {
            var users = await _userrep.GetAll();
            var user = users.FirstOrDefault(u => u.Name == name);
            if (user == null )
            {
                return Guid.Empty;
            }
            if (!_passwordHasher.CheckHashPassword(user, password))
            {
                return Guid.Empty;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleOfUser.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var httpContext = _httpContextAccessor.HttpContext!;

            await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

            return user.Id;
        }
    }
}
