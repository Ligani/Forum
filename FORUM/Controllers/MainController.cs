using DomainModels.Models;
using FORUM.Contracts;
using FORUM.ViewClass;
using Logics.Interfaces;
using Logics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace FORUM.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly IPostService _postService;
        public MainController(IUserService userService, IFileService fileService, IPostService postService)
        {
            _userService = userService;
            _fileService = fileService;
            _postService = postService;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userDomain = await _userService.GetUserById(currentUserId);
            var user = new UserResponse(userDomain.Id, userDomain.Name ,userDomain.About);

            var postsDomain = await _postService.GetPosts();
            var postsResponse = postsDomain.Select(p => new PostResponse(p.Id,p.Title, p.Content, p.FilePath, p.Created));

            var model = new MainViewModel
            {
                User = user,
                Posts = postsResponse
            };

            return View(model);
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> Profile(Guid id)
        {
            var userDomain = await _userService.GetUserById(id);
            if (userDomain == null)
                return NotFound();

            var user = new UserResponse(userDomain.Id, userDomain.Name, userDomain.About);

            var postsDomain = await _postService.GetPosts();
            var userPostsResponse = postsDomain.Where(p => p.User_Id == user.Id)
                                             .Select(p =>new PostResponse(p.Id,p.Title, p.Content, p.FilePath, p.Created));
            var model = new MainViewModel
            {
                User = user,
                Posts = userPostsResponse
            };

            return View("Account", model); 
        }
           

        [HttpPost]
        public async Task<IActionResult> NewPost(string title, string content, IFormFile file)
        {
            var filepath = await _fileService.Upload(file);
            var (post, error) = Post.CreatePost(Guid.NewGuid(), Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), title, content, DateTime.Now, filepath);
            if (!String.IsNullOrEmpty(error))
            {
                ViewBag.PostError = error;

                var postsDomain = await _postService.GetPosts();
                var postsResponse = postsDomain.Select(p => new PostResponse(p.Id, p.Title, p.Content, p.FilePath, p.Created));
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userDomain = await _userService.GetUserById(currentUserId);
                if (userDomain == null)
                    return NotFound();
               

                var user = new UserResponse(userDomain.Id, userDomain.Name, userDomain.About);
                var model = new MainViewModel
                {
                    User = user,
                    Posts = postsResponse
                };
                return View("Index", model);
            }
            await _postService.CreatePost(post);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid id_post, Guid id_user)
        {
            await _postService.DeletePost(id_post);
            return RedirectToAction("profile", new { id = id_user });   
        }
    }
}
