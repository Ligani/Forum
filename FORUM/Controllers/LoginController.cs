using DomainModels.Enums;
using DomainModels.Models;
using FORUM.Contracts;
using Logics.Interfaces;
using Logics.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FORUM.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRequest userReq)
        {
            var (validUser, error) = User_.CreateUser(Guid.NewGuid(), userReq.name, Role.User, userReq.password, "Пусто");
            if (!String.IsNullOrEmpty(error))
            {
                ViewBag.ErrorMessage = error;
                return View("Register");
            }
            var res = await _userService.UserReg(validUser);
            if (res==Guid.Empty)
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
            var id = await _userService.UserLogin(userRequest.name, userRequest.password);
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
    }
}
