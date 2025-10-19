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

        public async Task<List<User_>> GetAll()
        {
            var usersEntiity = await _context.Users.AsNoTracking().ToListAsync();
            var usersDomain = usersEntiity.Select(u => User_.CreateUser(u.Id,u.Name,u.RoleOfUser,u.HashPassword).user).ToList();
            return usersDomain;
        }

        public async Task<Guid> Create(User_ user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                Name = user.Name,
                HashPassword = user.HashPassword,
                RoleOfUser = user.RoleOfUser,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            return id;
        }
        public async Task<Guid> Update(Guid id, string name)
        {
            await _context.Users.Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(u => u.Name, u => name));

            return id;
        }
    }
}
