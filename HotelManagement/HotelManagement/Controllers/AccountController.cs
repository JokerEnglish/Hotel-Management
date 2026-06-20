using HotelManagement.InterfacesRepositories;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITaikhoanRepository _taikhoanRepo;
        private readonly HotelDbContext _context;

        public AccountController(ITaikhoanRepository taikhoanRepo, HotelDbContext context)
        {
            _taikhoanRepo = taikhoanRepo;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kiểm tra nhân viên trước
            var user = await _taikhoanRepo.GetByUsernameAndPasswordAsync(username, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.ManvNavigation.Hoten),
                    new Claim(ClaimTypes.Role, user.Vaitro)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra khách hàng
            var khach = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Email == username && k.MatKhau == password);
            if (khach != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, khach.Tenkh),
                    new Claim(ClaimTypes.Role, "Customer"),
                    new Claim("CustomerId", khach.Makh.ToString()),
                    new Claim("CustomerCCCD", khach.Cmndkh)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            // Đăng nhập sai
            ViewBag.ErrorMessage = "Tên đăng nhập/Email hoặc mật khẩu không đúng!";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Maloaikhach = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Loaikhaches, "Maloaikhach", "Tenloaikhach");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(HotelManagement.ViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Khachhangs.Any(k => k.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                }
                else
                {
                    var khachhang = new Khachhang
                    {
                        Tenkh = model.Tenkh,
                        Cmndkh = model.Cmndkh,
                        Tuoi = model.Tuoi,
                        Tel = model.Tel,
                        Diachikh = model.Diachikh,
                        Maloaikhach = model.Maloaikhach,
                        Email = model.Email,
                        MatKhau = model.MatKhau,
                        Map = null
                    };

                    _context.Khachhangs.Add(khachhang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
            }

            ViewBag.Maloaikhach = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Loaikhaches, "Maloaikhach", "Tenloaikhach", model.Maloaikhach);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
