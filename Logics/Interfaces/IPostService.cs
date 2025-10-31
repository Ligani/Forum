using DomainModels.Models;

namespace Logics.Interfaces
{
    public interface IPostService
    {
        Task<Guid> CreatePost(Post post);
        Task<Guid> DeletePost(Guid id);
        Task<List<Post>> GetPosts();
        Task<Post> GetPost(Guid id);
        Task<Guid> UpdatePost(Guid id, string title, string content, string filePath);
    }
}