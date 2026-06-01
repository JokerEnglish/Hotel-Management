using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITaikhoanRepository _taikhoanRepo;

        public AccountController(ITaikhoanRepository taikhoanRepo)
        {
            _taikhoanRepo = taikhoanRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _taikhoanRepo.GetByUsernameAndPasswordAsync(username, password);

            // Đổi lại thành != null (Tức là Đăng nhập ĐÚNG)
            if (user != null)
            {
                // Đây là phần code "Cấp thẻ từ" bạn lỡ quên:
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.ManvNavigation.Hoten),
                    new Claim(ClaimTypes.Role, user.Vaitro)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Đăng nhập đúng thì mới cho vào trang Home
                return RedirectToAction("Index", "Home");
            }

            // Đăng nhập sai: Báo lỗi
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Thu hồi "Thẻ từ"
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Đẩy ra ngoài trang Đăng nhập
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
