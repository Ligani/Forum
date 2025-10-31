using DomainModels.Enums;
using DomainModels.Models;
using FORUM.Contracts;
using FORUM.ViewClass;
using Logics.Interfaces;
using Logics.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FORUM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        public AccountController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRequest userReq)
        {
            var (validUser, error) = DomainModels.Models.User.CreateUser(Guid.NewGuid(), userReq.name, Role.User, userReq.password, "Пусто");
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ErrorMessage = error;
                return View("Register");
            }

            var res = await _userService.RegisterUser(validUser);

            if (res == Guid.Empty)
            {
                ViewBag.ErrorMessage = "Имя занято";
                return View("Register");
            }

            return RedirectToAction("Index");
        }

        public IActionResult RegisterView()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserRequest userRequest)
        {
            var id = await _userService.LoginUser(userRequest.name, userRequest.password);

            if (id == Guid.Empty)
            {
                return View("Index");
            }

            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public async Task<IActionResult> LogoutUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> Profile(Guid id)
        {
            var userDomain = await _userService.GetUser(id);
            if (userDomain == null)
                return NotFound();

            var user = new UserResponse(userDomain.Id, userDomain.Name, userDomain.About);

            var postsDomain = await _postService.GetPosts();
            var userPostsResponse = postsDomain.Where(p => p.User_Id == user.Id)
                                             .Select(p => new PostResponse(p.Id, p.Title, p.Content, p.FilePath, p.Created));
            var model = new MainViewModel
            {
                User = user,
                Posts = userPostsResponse
            };

            return View("Account", model);
        }
        [Authorize]
        public async Task<IActionResult> UpdateUserForm(Guid id, string newName, string newAbout)
        {
            await _userService.UpdateUser(id, newName, newAbout);

            return RedirectToAction("Index", "Main");
        }

        [Authorize]
        [HttpGet("update/{id:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid id)
        {
            var userDomain = await _userService.GetUser(id);
            if (userDomain == null)
                return NotFound();

            var user = new UserResponse(userDomain.Id, userDomain.Name, userDomain.About);

            var postsDomain = await _postService.GetPosts();
            var userPostsResponse = postsDomain.Where(p => p.User_Id == user.Id)
                                             .Select(p => new PostResponse(p.Id, p.Title, p.Content, p.FilePath, p.Created));
            var model = new MainViewModel
            {
                User = user,
                Posts = userPostsResponse
            };
            return View("UpdateUser", model);
        }
    }
}
