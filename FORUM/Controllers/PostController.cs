using DomainModels.Models;
using FORUM.Contracts;
using FORUM.ViewClass;
using Logics.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FORUM.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public PostController(IUserService userService, IPostService postService, IFileService fileService)
        {
            _userService = userService;
            _postService = postService;
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(string title, string content, IFormFile file)
        {
            var filepath = await _fileService.UploadAsync(file);
            var (post, error) = Post.CreatePost(Guid.NewGuid(), Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), title, content, DateTime.Now, filepath);
            if (!String.IsNullOrEmpty(error))
            {
                ViewBag.PostError = error;

                var postsDomain = await _postService.GetPostsAsync();
                var postsResponse = postsDomain.Select(p => new PostResponse(p.Id, p.Title, p.Content, p.FilePath, p.Created));
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userDomain = await _userService.GetUserAsync(currentUserId);
                if (userDomain == null)
                    return NotFound();


                var user = new UserResponse(userDomain.Id, userDomain.Name, userDomain.About);
                var model = new MainViewModel
                {
                    User = user,
                    Posts = postsResponse
                };
                return RedirectToAction("Index","Main", model);
            }
            await _postService.CreatePostAsync(post);
            return RedirectToAction("Index", "Main");
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid id_post, Guid id_user)
        {
            var post = await _postService.GetPostAsync(id_post);
            await _postService.DeletePostAsync(id_post);
            await _fileService.DeleteAsync(post.FilePath);
            return RedirectToAction("profile", "Account", new { id = id_user });
        }
    }
}
