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
        public Task<Guid> RegisterUser(User newuser);
        public Task<Guid> LoginUser(string name, string password);
        Task<List<User>> GetUsers();
        Task<User?> GetUser(Guid Id);
        Task<Guid> UpdateUser(Guid id, string newName, string newAbout);
    }
}
