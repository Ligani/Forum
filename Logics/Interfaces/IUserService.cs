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
        public Task<Guid> UserReg(User_ newuser);
        public Task<Guid> UserLogin(string name, string password);
        Task<List<User_>> GetAllUsers();
        Task<User_?> GetUserById(Guid Id);
    }
}
