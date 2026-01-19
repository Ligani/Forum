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

        public async Task<Guid> CreateAsync(Post post)
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

        public async Task<List<Post>> GetAllAsync()
        {
            var postsEntity = await _context.Posts.AsNoTracking().ToListAsync();
            var postsDomain = postsEntity.Select(p => Post.CreatePost(p.Id, p.User_Id, p.Title, p.Content, p.Created, p.FilePath).post).ToList();
            return postsDomain;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            await _context.Posts.Where(p => p.Id==id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> UpdateAsync(Guid id, string title, string content, string filePath)
        {
            await _context.Posts.Where(p => p.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Title, p => title)
                .SetProperty(p => p.Content, p => content)
                .SetProperty(p => p.FilePath, p => filePath));

            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var postEntity = await _context.Posts.FindAsync(id);
            var postDomain = Post.CreatePost(postEntity.Id,postEntity.User_Id,postEntity.Title,postEntity.Content, postEntity.Created, postEntity.FilePath).post;
            return postDomain;
        }
    }
}
