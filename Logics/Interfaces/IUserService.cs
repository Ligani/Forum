using DomainModels.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> RegisterUserAsync(User newuser);
        public Task<Guid> LoginUserAsync(string name, string password);
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserAsync(Guid Id);
        Task<Guid> UpdateUserAsync(Guid id, string newName, string newAbout);
    }
}
