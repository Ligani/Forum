using DomainModels.Models;

namespace Logics.Interfaces
{
    public interface IPostService
    {
        Task<Guid> CreatePostAsync(Post post);
        Task<Guid> DeletePostAsync(Guid id);
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostAsync(Guid id);
        Task<Guid> UpdatePostAsync(Guid id, string title, string content, string filePath);
    }
}