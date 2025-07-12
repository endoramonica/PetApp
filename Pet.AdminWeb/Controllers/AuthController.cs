using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pet.AdminWeb.Services;
using Pet.Core.Application.DTOs;

namespace Pet.AdminWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View(dto);
            }

            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}
