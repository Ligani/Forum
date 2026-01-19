using DataAccess.Repositories;
using DomainModels.Models;
using Logics.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics.Services
{
    public class PostService : IPostService
    {
        private readonly PostRepository _postRep;
        public PostService(PostRepository postRepository)
        {
            _postRep = postRepository;
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            return await _postRep.GetAllAsync();
        }
        public async Task<Post> GetPostAsync(Guid id)
        {
            return await _postRep.GetAsync(id);
        }
        public async Task<Guid> CreatePostAsync(Post post)
        {
            await _postRep.CreateAsync(post);
            return post.Id;
        }

        public async Task<Guid> DeletePostAsync(Guid id)
        {
            await _postRep.DeleteAsync(id);
            return id;
        }

        public async Task<Guid> UpdatePostAsync(Guid id, string title, string content, string filePath)
        {
            await _postRep.UpdateAsync(id,title,content,filePath);
            return id;
        }
    }
}
