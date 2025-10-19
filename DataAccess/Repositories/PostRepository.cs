using DataAccess.Contexts;
using DataAccess.Entities;
using DomainModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Post post)
        {
            var postEntity = new PostEntity()
            {
                Id = post.Id,
                User_Id = post.User_Id,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created,
                FilePath = post.FilePath,
            };
            await _context.Posts.AddAsync(postEntity);
            await _context.SaveChangesAsync();

            return postEntity.Id;
        }

        public async Task<List<Post>> GetAll()
        {
            var postsEntity = await _context.Posts.AsNoTracking().ToListAsync();
            var postsDomain = postsEntity.Select(p => Post.CreatePost(p.Id, p.User_Id, p.Title, p.Content, p.Created, p.FilePath).post).ToList();
            return postsDomain;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Posts.Where(p => p.Id==id).ExecuteDeleteAsync();
            return id;
        }

        public async Task<Guid> Update(Guid id, string title, string content, string filePath)
        {
            await _context.Posts.Where(p => p.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Title, p => title)
                .SetProperty(p => p.Content, p => content)
                .SetProperty(p => p.FilePath, p => filePath));
            return id;
        }
    }
}
