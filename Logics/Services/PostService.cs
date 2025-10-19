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
        public async Task<List<Post>> GetPosts()
        {
            return await _postRep.GetAll();
        }
        public async Task<Guid> CreatePost(Post post)
        {
            await _postRep.Create(post);
            return post.Id;
        }

        public async Task<Guid> DeletePost(Guid id)
        {
            await _postRep.Delete(id);
            return id;
        }

        public async Task<Guid> UpdatePost(Guid id, string title, string content, string filePath)
        {
            await _postRep.Update(id,title,content,filePath);
            return id;
        }
    }
}
