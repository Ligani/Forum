using DataAccess.Contexts;
using DataAccess.Entities;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository
    {
        public readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var usersEntiity = await _context.Users.AsNoTracking().ToListAsync();
            var usersDomain = usersEntiity.Select(u => User.CreateUser(u.Id,u.Name,u.RoleOfUser,u.HashPassword, u.About).user).ToList();
            return usersDomain;
        }

        public async Task<Guid> CreateAsync(User user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                Name = user.Name,
                HashPassword = user.HashPassword,
                RoleOfUser = user.RoleOfUser,
                About = user.About,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }
        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();

            return id;
        }
        public async Task<Guid> UpdateAsync(Guid id, string name,string about)
        {
            await _context.Users.Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(u => u.Name, u => name)
                .SetProperty(u => u.About, u => about));
            
            return id;
        }
        public async Task<User> GetAsync(Guid id)
        {
            var userEntity = await _context.Users.FindAsync(id);
            var userDomain = User.CreateUser(userEntity.Id,userEntity.Name,userEntity.RoleOfUser,userEntity.HashPassword,userEntity.About).user;
            return userDomain;
        }
        public async Task<User> GetAsync(string name)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == name);

            if (userEntity == null)
                return null;

            return User.CreateUser(userEntity.Id, userEntity.Name, userEntity.RoleOfUser, userEntity.HashPassword, userEntity.About).user;
        }
    }
}
