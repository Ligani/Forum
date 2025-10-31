using DomainModels.Models;
using FORUM.Contracts;
using FORUM.ViewClass;
using Logics.Interfaces;
using Logics.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            var userDomain = await _userService.GetUser(currentUserId);
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
       
        public  async Task<IActionResult> ShowUsers()
        {
            var usersDomain = await _userService.GetUsers();
            var usersResponse = usersDomain.Select(u => new UserResponse(u.Id, u.Name, u.About));
            return View("AllUsersView", usersResponse);
        }
    }
}
