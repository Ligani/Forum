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
        private readonly UserRepository _userRep;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasherService _passwordHasher;
        public UserService(UserRepository userrep,
            IHttpContextAccessor httpContextAccessor,
            IPasswordHasherService passwordHasher) 
        {
            _userRep = userrep;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;

        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _userRep.GetAllAsync();
        }
        public async Task<User?> GetUserAsync(Guid id)
        {
            var user = await _userRep.GetAsync(id);

            return user;
        }
        public async Task<Guid> RegisterUserAsync(User newUser)
        {
            var checkName = await _userRep.GetAsync(newUser.Name);
            if (checkName != null)
            {
                return Guid.Empty;
            }
            var validUser = _passwordHasher.HashPassword(newUser.HashPassword, newUser);
            return await _userRep.CreateAsync(validUser);
        }
        public async Task<Guid> UpdateUserAsync(Guid id, string newName,string newAbout)
        {
            await _userRep.UpdateAsync(id, newName, newAbout);
            return id;
        }


        public async Task<Guid> LoginUserAsync(string name, string password)
        {
            var user =  await _userRep.GetAsync(name);
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
